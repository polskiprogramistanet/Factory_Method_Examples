using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Method_Example_1
{

        class Program
        {
            static void Main(string[] args)
            {

                try
                {
                    Console.WriteLine("Test fabryki abstrakcyjnej do budowy sekwensera rozkazów poszczególnych protokołów");
                    Console.WriteLine();
                    ISequencesFactory seqLogitronFactory = new Logitron_Fabryka();
                    ISequencesFactory seqEHLFactory = new MMPetro_Fabryka();

                    SequencerClient logitron = new SequencerClient(seqLogitronFactory);

                    SequencerClient ehl = new SequencerClient(seqEHLFactory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex.Message); ;
                }

                Console.ReadKey(true);
            }

        }


        class SequencerClient
        {
            SequenceStandByBase sequenceStandBy;
            SequenceTankCallBase sequenceTankCall;
            SequenceTestBase sequenceTest;

            public SequencerClient(ISequencesFactory sequencesFactory)
            {
                sequenceStandBy = sequencesFactory.Build_Seq_StandBy_Product();
                sequenceTankCall = sequencesFactory.Build_Seq_TankCAll_Product();
                sequenceTest = sequencesFactory.Build_Seq_Test_Product();
            }
        }

        #region "Logitron"
        class Logitron_Fabryka : ISequencesFactory
        {
            public SequenceStandByBase Build_Seq_StandBy_Product()
            {
                var standby = new Logitron_Seq_StandBy_Product();
                standby.AddCommand(new LogitronCommandGeneralPolling());
                return standby;
            }
            public SequenceTankCallBase Build_Seq_TankCAll_Product()
            {
                var tankCall = new Logitron_Seq_TankCall_Product();
                tankCall.AddCommand(new LogitronCommandGeneralPolling());
                tankCall.AddCommand(new LogitronCommandIdentyfication());
                return tankCall;
            }
            public SequenceTestBase Build_Seq_Test_Product()
            {
                var test = new Logitron_Seq_Test_Product();
                test.AddCommand(new LogitronCommandGeneralPolling());
                test.AddCommand(new LogitronCommandPumpStatus());
                return test;
            }
        }

        #region "Logitron Sequences"
        class Logitron_Seq_StandBy_Product : SequenceStandByBase
        {
            public Queue<ILogitronCommand> Commands { get; private set; }
            ILogitronCommand cmd;
            public Logitron_Seq_StandBy_Product()
            {
                this.Commands = new Queue<ILogitronCommand>();
            }
            public override void AddCommand(ICommands command)
            {
                this.cmd = (ILogitronCommand)command;
                this.Commands.Enqueue(this.cmd);
                Console.WriteLine($"do sekwencji logitron StandBy dodano komendę: {this.cmd.Name}");
            }
        }

        class Logitron_Seq_TankCall_Product : SequenceTankCallBase
        {
            public Queue<ILogitronCommand> Commands { get; private set; }
            ILogitronCommand cmd;
            public Logitron_Seq_TankCall_Product()
            {
                this.Commands = new Queue<ILogitronCommand>();
            }
            public override void AddCommand(ICommands command)
            {
                this.cmd = (ILogitronCommand)command;
                this.Commands.Enqueue(cmd);
                Console.WriteLine($"do sekwencji logitron TankCall dodano komendę: {cmd.Name}");
            }
        }

        class Logitron_Seq_Test_Product : SequenceTestBase
        {
            public Queue<ILogitronCommand> Commands { get; private set; }
            ILogitronCommand cmd;
            public Logitron_Seq_Test_Product()
            {
                this.Commands = new Queue<ILogitronCommand>();
            }

            public override void AddCommand(ICommands command)
            {
                cmd = (ILogitronCommand)command;
                this.Commands.Enqueue(cmd);
                Console.WriteLine($"do sekwencji logitron Test dodano komendę: {cmd.Name}");
            }
        }
        #endregion

        #region "Logitron Commands"
        public interface ILogitronCommand : ICommands
        {
            LogitronCommandEnum Name { get; }
            //byte[] Value { get; }
        }
        class LogitronCommandGeneralPolling : ILogitronCommand
        {
            byte[] buffor = new byte[] { 0x04, 0x30, 0x31, 0x02, 0x32, 0x50, 0x03, 0x62 };
            public LogitronCommandEnum Name => LogitronCommandEnum.General_Polling;
            public byte[] Value => buffor;
        }
        class LogitronCommandPumpStatus : ILogitronCommand
        {
            byte[] buffor = new byte[] { 0x04, 0x30, 0x30, 0x02, 0x30, 0x53, 0x30, 0x03, 0x52 };
            public LogitronCommandEnum Name => LogitronCommandEnum.Pump_Status;
            public byte[] Value => buffor;
        }
        class LogitronCommandIdentyfication : ILogitronCommand
        {
            byte[] buffor = new byte[] { 0x04, 0x30, 0x31, 0x02, 0x34, 0x49, 0x03, 0x7d };
            public LogitronCommandEnum Name => LogitronCommandEnum.Identyfication;
            public byte[] Value => buffor;
        }
        #endregion

        public enum LogitronCommandEnum { General_Polling, Pump_Status, Identyfication }
        #endregion


        #region "MMPetro"
        class MMPetro_Fabryka : ISequencesFactory
        {
            public SequenceStandByBase Build_Seq_StandBy_Product()
            {
                var standBy = new EHL_Seq_StandBy_Product();
                standBy.AddCommand(new EhlCommandState());
                return standBy;
            }

            public SequenceTankCallBase Build_Seq_TankCAll_Product()
            {
                var tankCall = new EHL_Seq_TankCAll_Product();
                tankCall.AddCommand(new EhlCommandState());
                tankCall.AddCommand(new EhlCommandVolume());
                return tankCall;
            }

            public SequenceTestBase Build_Seq_Test_Product()
            {
                var test = new EHL_Seq_Test_Product();
                test.AddCommand(new EhlCommandLineTest());
                test.AddCommand(new EhlCommandState());
                return test;
            }
        }

        #region "EHL Sequences"
        class EHL_Seq_StandBy_Product : SequenceStandByBase
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            IEHLCommand cmd;
            public EHL_Seq_StandBy_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public override void AddCommand(ICommands command)
            {
                this.cmd = (IEHLCommand)command;
                this.Commands.Enqueue(cmd);
                Console.WriteLine($"do sekwencji ehl StandBy dodano komendę: {cmd.Name}");
            }
        }
        class EHL_Seq_TankCAll_Product : SequenceTankCallBase
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            IEHLCommand cmd;
            public EHL_Seq_TankCAll_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public override void AddCommand(ICommands command)
            {
                this.cmd = (IEHLCommand)command;
                this.Commands.Enqueue(this.cmd);
                Console.WriteLine($"do sekwencji ehl TankCall dodano komendę: {this.cmd.Name}");
            }
        }
        class EHL_Seq_Test_Product : SequenceTestBase
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            IEHLCommand cmd;
            public EHL_Seq_Test_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public override void AddCommand(ICommands command)
            {
                this.cmd = (IEHLCommand)command;
                this.Commands.Enqueue(cmd);
                Console.WriteLine($"do sekwencji ehl Test dodano komendę: {cmd.Name}");
            }
        }
        #endregion

        #region "EHL Commands"
        public interface IEHLCommand : ICommands
        {
            EHLCommandEnum Name { get; }
            //byte[] Value { get; }
        }
        class EhlCommandState : IEHLCommand
        {
            byte[] buffor = new byte[] { 0x10, 0x06, 0x21, 0x4B, 0x7C, 0x36 };
            public EHLCommandEnum Name => EHLCommandEnum.State;

            public byte[] Value => buffor;


        }
        class EhlCommandLineTest : IEHLCommand
        {
            byte[] buffor = new byte[] { 0x10, 0x06, 0x21, 0x6A, 0x5D, 0x36 };
            public EHLCommandEnum Name => EHLCommandEnum.LineTest;

            public byte[] Value => buffor;
        }
        class EhlCommandVolume : IEHLCommand
        {
            byte[] buffor = new byte[] { 0x10, 0x06, 0x21, 0x45, 0x72, 0x36 };
            public EHLCommandEnum Name => EHLCommandEnum.Volume;

            public byte[] Value => buffor;
        }
        #endregion
        public enum EHLCommandEnum { State, LineTest, Volume }
        #endregion


        #region "Refactoring"

        interface ISequencesFactory
        {
            SequenceStandByBase Build_Seq_StandBy_Product();
            SequenceTankCallBase Build_Seq_TankCAll_Product();
            SequenceTestBase Build_Seq_Test_Product();
        }

        public abstract class SequenceStandByBase
        {
            //public Queue<ICommandProtocol> Commands { get; set; }
            public abstract void AddCommand(ICommands command);
        }
        public abstract class SequenceTankCallBase
        {
            //public Queue<ICommandProtocol> Commands { get; set; }
            public abstract void AddCommand(ICommands command);
        }
        public abstract class SequenceTestBase
        {
            //public Queue<ICommandProtocol> Commands { get; set; }
            public abstract void AddCommand(ICommands command);
        }

        public interface ICommands
        {
            //string Name { get; }

            byte[] Value { get; }
        }

        public class Sequence_StandBy_Product
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            public Sequence_StandBy_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public void AddCommand(IEHLCommand command)
            {
                this.Commands.Enqueue(command);
                Console.WriteLine($"do sekwencji StandBy dodano komendę: {command.Name}");
            }
        }
        public class Sequence_TankCall_Product
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            public Sequence_TankCall_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public void AddCommand(IEHLCommand command)
            {
                this.Commands.Enqueue(command);
                Console.WriteLine($"do sekwencji TankCall dodano komendę: {command.Name}");
            }
        }
        public class Sequence_Test_Product
        {
            public Queue<IEHLCommand> Commands { get; private set; }
            public Sequence_Test_Product()
            {
                this.Commands = new Queue<IEHLCommand>();
            }
            public void AddCommand(IEHLCommand command)
            {
                this.Commands.Enqueue(command);
                Console.WriteLine($"do sekwencji Test dodano komendę: {command.Name}");
            }
        }

        #endregion
    }

