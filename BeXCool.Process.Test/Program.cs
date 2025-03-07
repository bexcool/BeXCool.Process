using BeXCool.Process;

namespace BeXCool.Process.Test
{
    public static class Program
    {
        enum Test
        {
            None,
            Test1,
            Test2,
        }

        public static void Main()
        {
            var ml = MessageListener.Listen("test_1234", (id, message) =>
            {
                Console.WriteLine("TEST");
            });

            MessageSender.SendMessage("test_1234", new MessageModel((long)Test.Test1));

            while(true) { }
        }
    }
}