using BFCore.Command;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Extension
{
    [TestClass]
    public class BFCommandTypeExtensionsTest : TestBase
    {
        [TestMethod]
        [TestCase(BFCommandType.Increment, true)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void IncrementJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Increment.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, true)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void DecrementJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Decrement.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, true)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void MoveRightJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.MoveRight.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, true)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void MoveLeftJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.MoveLeft.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, true)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void RoopHeadJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.RoopHead.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, true)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void RoopTaleJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.RoopTale.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, true)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void ReadJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Read.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, true)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void WriteJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Write.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, true)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void BeginCommentJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.BeginComment.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, true)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, false)]
        public void EndCommentJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.EndComment.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, true)]
        [TestCase(BFCommandType.Undefined, false)]
        public void TriviaJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Trivia.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }

        [TestMethod]
        [TestCase(BFCommandType.Increment, false)]
        [TestCase(BFCommandType.Decrement, false)]
        [TestCase(BFCommandType.MoveRight, false)]
        [TestCase(BFCommandType.MoveLeft, false)]
        [TestCase(BFCommandType.RoopHead, false)]
        [TestCase(BFCommandType.RoopTale, false)]
        [TestCase(BFCommandType.Read, false)]
        [TestCase(BFCommandType.Write, false)]
        [TestCase(BFCommandType.BeginComment, false)]
        [TestCase(BFCommandType.EndComment, false)]
        [TestCase(BFCommandType.Trivia, false)]
        [TestCase(BFCommandType.Undefined, true)]
        public void UndefinedJudgeCorrectlyTest()
        {
            this.TestContext.Run<BFCommandType, bool>((testCase, expected) =>
            {
                BFCommandType.Undefined.IsCommandTypeOf(testCase).Is(expected, $@"TestCase: ""{testCase}"" Expected: ""{expected}""");
            });
        }
    }
}
