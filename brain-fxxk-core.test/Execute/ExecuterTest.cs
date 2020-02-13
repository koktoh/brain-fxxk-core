using BFCore.Config;
using BFCore.Execute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Execute
{
    [TestClass]
    public class ExecuterTest
    {
        private TestExecuter _executer;

        [TestInitialize]
        public void Init()
        {
            var config = new CommonConfig();
            var commandConfig = new BFCommandConfig();
            this._executer = new TestExecuter(config, commandConfig);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            // Hello World!
            var src = "+++++++++[>++++++++>+++++++++++>+++<<<-]>.>++.+++++++..+++.>+++++.<<<+++++[>+++<-]>.>.+++.------.--------.>+.";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteMultiLineTest()
        {
            // Hello World!
            var src =
@"+++++++++[>++++++++>+++++++++++>+++<<<-]>.
>++.
+++++++.
.
+++.
>+++++.
<<<+++++[>+++<-]>.
>.
+++.
------.
--------.
>+.";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteWithTrailingCommentTest()
        {
            // Hello World!
            var src = @"+++++++++[>++++++++>+++++++++++>+++<<<-]>.>++.+++++++..+++.>+++++.<<<+++++[>+++<-]>.>.+++.------.--------.>+. # Hello World!";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteWithInLineCommentTest()
        {
            // Hello World!
            var src = @"+++++++++[>++++++++>+++++++++++>+++<<<-]>.#H;>++.#e;+++++++.#l;.#l;+++.#o;>+++++.# ;<<<+++++[>+++<-]>.#W;>.#o;+++.#r;------.#l;--------.#d;>+.#!;";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteWithInLineCommentInsideCommandsTest()
        {
            // Hello World!
            var src = @"+++++++++[>++++++++>+++++++++++>+++<<<-]>.#+;>++.#-;+++++++.#>;.#<;+++.#[;>+++++.#];<<<+++++[>+++<-]>.##;>.#<<<+++++[>+++<-]>.;+++.------.--------.>+.";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteMultiLineWithTrailingCommentTest()
        {
            // Hello World!
            var src =
@"+++++++++[>++++++++>+++++++++++>+++<<<-]>.#H
>++.#e
+++++++.#l
.#l
+++.#o
>+++++.# 
<<<+++++[>+++<-]>.#W
>.#o
+++.#r
------.#l
--------.#d
>+.#!";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteWithCommentRowTest()
        {
            // Hello World!
            var src =
@"# Output ""Hello World!""
+++++++++[>++++++++>+++++++++++>+++<<<-]>.
>++.
+++++++.
.
+++.
>+++++.
<<<+++++[>+++<-]>.
>.
+++.
------.
--------.
>+.";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteWithCommentRowInsideCommandsTest()
        {
            // Hello World!
            var src =
@"#+
+++++++++[>++++++++>+++++++++++>+++<<<-]>.
#-
>++.
#>
+++++++.
#<
.
#[
+++.
#]
>+++++.
##
<<<+++++[>+++<-]>.
#<<<+++++[>+++<-]>.
>.
+++.
------.
--------.
>+.";

            this._executer.Execute(src);

            this._executer.Text.Is("Hello World!");
        }

        [TestMethod]
        public void ExecuteNestedLoopTest()
        {
            // @@
            var src = @"++++[>++++[>++++<-]<-]>>..";

            this._executer.Execute(src);

            this._executer.Text.Is("@@");
        }

    }
}