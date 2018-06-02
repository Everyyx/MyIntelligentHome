using MyIntelligentHomeSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace MyIntelligentHomeSystem.Helpers
{
    public class I2CHelper
    {
        private static bool Lock = false;
        private static string AQS;
        private static DeviceInformationCollection DIS;
        public enum Mode : byte
        {
            /// <summary>
            /// 获取传感器数据
            /// </summary>
            Mode0 = 0,
            /// <summary>
            /// 获取设备状态
            /// </summary>
            Mode1 = 1,
            /// <summary>
            /// 发送IO控制信号给Arduino
            /// </summary>
            Mode2 = 2
        }

        /// <summary>
        /// 向目标设备发送控制信号
        /// </summary>
        /// <param name="_Room">目标位置</param>
        /// <param name="ControlMode"></param>
        /// <param name="Pin">需要设置低高电压的Pin.(Mode2)</param>
        /// <param name="PinValue">0?1.（Mode2）</param>
        /// <returns>14位的返回数值.</returns>
        public static async System.Threading.Tasks.Task<byte[]> WriteRead(Room _Room, Mode ControlMode, byte Pin = 0, byte PinValue = 0)
        {
            while (Lock != false)
            {

            }

            Lock = true;

            byte[] Response = new byte[14];

            try
            {

                var Settings = new I2cConnectionSettings(_Room.I2C_Slave_Address)
                {
                    BusSpeed = I2cBusSpeed.StandardMode
                };

                if (AQS == null || DIS == null)
                {
                    AQS = I2cDevice.GetDeviceSelector("I2C1");
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    Device.Write(new byte[] { byte.Parse(ControlMode.ToString().Replace("Mode", "")), Pin, PinValue });

                    Device.Read(Response);
                }
            }
            catch (Exception)
            {

            }

            Lock = false;
            return Response;
        }
        public static async Task<byte[]> WriteRead(int I2C_Slave_Address, Mode ControlMode, byte Pin = 0, byte PinValue = 0)
        {
            while (Lock != false)
            {
            }
            Lock = true;
            byte[] Response = new byte[14];
            try
            {
                // 初始化I2C
                var Settings = new I2cConnectionSettings(I2C_Slave_Address)
                {
                    BusSpeed = I2cBusSpeed.StandardMode
                };
                if (AQS == null || DIS == null)
                {
                    AQS = I2cDevice.GetDeviceSelector("I2C1");
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }
                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    Device.Write(new byte[] { byte.Parse(ControlMode.ToString().Replace("Mode", "")), Pin, PinValue });

                    Device.Read(Response);
                }
            }
            catch (Exception)
            {
            }
            Lock = false;
            return Response;
        }

    }
}
