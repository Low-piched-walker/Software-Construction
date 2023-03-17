using System;

namespace AlarmClock
{
    // 定义嘀嗒事件参数类
    public class TickEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }

        public TickEventArgs(DateTime time)
        {
            Time = time;
        }
    }

    // 定义响铃事件参数类
    public class AlarmEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public AlarmEventArgs(string message)
        {
            Message = message;
        }
    }

    // 定义闹钟类
    public class AlarmClock
    {
        public event EventHandler<TickEventArgs> Tick;
        public event EventHandler<AlarmEventArgs> Alarm;

        private bool isAlarmSet = false;
        private DateTime alarmTime;

        // 设置闹钟时间
        public void SetAlarmTime(DateTime time)
        {
            alarmTime = time;
            isAlarmSet = true;
        }

        // 启动闹钟
        public void Start()
        {
            while (true)
            {
                DateTime now = DateTime.Now;

                // 触发嘀嗒事件
                Tick?.Invoke(this, new TickEventArgs(now));

                if (isAlarmSet && now >= alarmTime)
                {
                    // 触发响铃事件
                    Alarm?.Invoke(this, new AlarmEventArgs("Time's up!"));

                    // 关闭闹钟
                    isAlarmSet = false;
                    break;
                }

                // 等待一秒钟
                System.Threading.Thread.Sleep(1000);
            }
        }
    }

    // 测试闹钟类
    class Program
    {
        static void Main(string[] args)
        {
            AlarmClock alarmClock = new AlarmClock();

            // 订阅嘀嗒事件
            alarmClock.Tick += (sender, e) => Console.WriteLine("Tick: {0}", e.Time);

            // 订阅响铃事件
            alarmClock.Alarm += (sender, e) => Console.WriteLine("Alarm: {0}", e.Message);

            // 设置闹钟时间为当前时间加30秒
            alarmClock.SetAlarmTime(DateTime.Now.AddSeconds(30));

            // 启动闹钟
            alarmClock.Start();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
