namespace BFCore.Command
{
    public enum BFCommandType
    {
        Undefined,      // 不明
        Trivia,         // 空白、コメントなど、実行に関係ないもの
        Increment,      // 現在のメモリに +1
        Decrement,      // 現在のメモリに -1
        MoveRight,      // 1つ右のメモリに移動
        MoveLeft,       // 1つ左のメモリに移動
        LoopHead,       // ループ始端
        LoopTail,       // ループ終端
        Read,           // 標準入力を1文字読む
        Write,          // 標準出力に1文字出力
        BeginComment,   // コメント開始
        EndComment,     // コメント終了
    }
}
