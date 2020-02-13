using BFCore.Config;
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

        [TestMethod]
        public void ExecuteFizzBuzzTest()
        {
            // FizzBuzz
            // from: https://github.com/pablojorge/brainfuck/blob/master/programs/bizzfuzz.bf
            var src =
@"++++++++++[>++++++++++<-]>>++++++++++>->>>>>>>>>>>>>>>>-->+++++++[->++
++++++++<]>[->+>+>+>+<<<<]+++>>+++>>>++++++++[-<++++<++++<++++>>>]++++
+[-<++++<++++>>]>>-->++++++[->+++++++++++<]>[->+>+>+>+<<<<]+++++>>+>++
++++>++++++>++++++++[-<++++<++++<++++>>>]++++++[-<+++<+++<+++>>>]>>-->
---+[-<+]-<[+[->+]-<<->>>+>[-]++[-->++]-->+++[---++[--<++]---->>-<+>[+
+++[----<++++]--[>]++[-->++]--<]>++[--+[-<+]->>[-]+++++[---->++++]-->[
->+<]>>[.>]++[-->++]]-->+++]---+[-<+]->>-[+>>>+[-<+]->>>++++++++++<<[-
>+>-[>+>>]>[+[-<+>]>+>>]<<<<<<]>>[-]>>>++++++++++<[->-[>+>>]>[+[-<+>]>
+>>]<<<<<]>[-]>>[>++++++[-<++++++++>]<.<<+>+>[-]]<[<[->-<]++++++[->+++
+++++<]>.[-]]<<++++++[-<++++++++>]<.[-]<<[-<+>]+[-<+]->>]+[-]<<<.>>>+[
-<+]-<<]";

            var expected = "1\n2\nFizz\n4\nBuzz\nFizz\n7\n8\nFizz\nBuzz\n11\nFizz\n13\n14\nFizzBuzz\n16\n17\nFizz\n19\nBuzz\nFizz\n22\n23\nFizz\nBuzz\n26\nFizz\n28\n29\nFizzBuzz\n31\n32\nFizz\n34\nBuzz\nFizz\n37\n38\nFizz\nBuzz\n41\nFizz\n43\n44\nFizzBuzz\n46\n47\nFizz\n49\nBuzz\nFizz\n52\n53\nFizz\nBuzz\n56\nFizz\n58\n59\nFizzBuzz\n61\n62\nFizz\n64\nBuzz\nFizz\n67\n68\nFizz\nBuzz\n71\nFizz\n73\n74\nFizzBuzz\n76\n77\nFizz\n79\nBuzz\nFizz\n82\n83\nFizz\nBuzz\n86\nFizz\n88\n89\nFizzBuzz\n91\n92\nFizz\n94\nBuzz\nFizz\n97\n98\nFizz\nBuzz\n";

            this._executer.Execute(src);

            this._executer.Text.Is(expected);
        }
    }
}