using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Extension
{
    [TestClass]
    public class BFCommandExtensionsTest : TestBase
    {
        private static readonly BFCommandConfig _config = new BFCommandConfig();

        public static object[] incrementTestSource = new[]
        {
            new object[] { _config.Increment, true },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(incrementTestSource))]
        public void IsIncrementTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsIncrement().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] decrementTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, true },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(decrementTestSource))]
        public void IsDecrementTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsDecrement().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] moveRightTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, true },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(moveRightTestSource))]
        public void IsMoveRightTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsMoveRight().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] moveLeftTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, true },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(moveLeftTestSource))]
        public void IsMoveLeftTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsMoveLeft().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] roopHeadTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, true },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(roopHeadTestSource))]
        public void IsRoopHeadTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsRoopHead().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] roopTaleTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, true },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(roopTaleTestSource))]
        public void IsRoopTaleTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsRoopTale().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] readTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, true },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(readTestSource))]
        public void IsReadTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsRead().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] writeTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, true },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(writeTestSource))]
        public void IsWriteTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsWrite().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] beginCommentTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, true },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(beginCommentTestSource))]
        public void IsBeginCommentTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsBeginComment().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] endCommentTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, true },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(endCommentTestSource))]
        public void IsEndCommentTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsEndComment().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] triviaTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), true },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(triviaTestSource))]
        public void IsTriviaTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsTrivia().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] undefinedTestSource = new[]
        {
            new object[] { _config.Increment, false },
            new object[] { _config.Decrement, false },
            new object[] { _config.MoveRight, false },
            new object[] { _config.MoveLeft, false },
            new object[] { _config.RoopHead, false },
            new object[] { _config.RoopTale, false },
            new object[] { _config.Read, false },
            new object[] { _config.Write, false },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), true },
        };

        [TestMethod]
        [TestCaseSource(nameof(undefinedTestSource))]
        public void IsUndefinedTest()

        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsUndefined().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] executableTestSource = new[]
        {
            new object[] { _config.Increment, true },
            new object[] { _config.Decrement, true },
            new object[] { _config.MoveRight, true },
            new object[] { _config.MoveLeft, true },
            new object[] { _config.RoopHead, true },
            new object[] { _config.RoopTale, true },
            new object[] { _config.Read, true },
            new object[] { _config.Write, true },
            new object[] { _config.BeginComment, false },
            new object[] { _config.EndComment, false },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), false },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(executableTestSource))]
        public void IsExecutableTest()
        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsExecutable().Is(expected, $@"TestCase: ""{target}""");
            });
        }

        public static object[] definedCommandTestSource = new[]
        {
            new object[] { _config.Increment, true },
            new object[] { _config.Decrement, true },
            new object[] { _config.MoveRight, true },
            new object[] { _config.MoveLeft, true },
            new object[] { _config.RoopHead, true },
            new object[] { _config.RoopTale, true },
            new object[] { _config.Read, true },
            new object[] { _config.Write, true },
            new object[] { _config.BeginComment, true },
            new object[] { _config.EndComment, true },
            new object[] { new BFCommand(" ", BFCommandType.Trivia), true },
            new object[] { new BFCommand(null, BFCommandType.Undefined), false },
        };

        [TestMethod]
        [TestCaseSource(nameof(definedCommandTestSource))]
        public void IsDefinedCommandTest()
        {
            this.TestContext.Run<BFCommand, bool>((target, expected) =>
            {
                target.IsDefinedCommand().Is(expected, $@"TestCase: ""{target}""");
            });
        }
    }
}
