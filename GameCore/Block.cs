namespace Unicursal.GameCore
{
    class Block
    {
        public Block()
        {
            IsWall = false;
            IsTeleporter = false;
            RestrictFrom = Direction.None;
            RestrictTo = Direction.None;
            FromDirection = Direction.None;
        }
        /// <summary>
        /// 本块的序号
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// 本块是一块墙
        /// </summary>
        public bool IsWall { get; internal set; }
        /// <summary>
        /// 本块是否是一个传送点
        /// </summary>
        public bool IsTeleporter { get; internal set; }
        /// <summary>
        /// 如果块有方向约束，RestrictFrom约束了只能从哪个方向进入
        /// </summary>
        public Direction RestrictFrom { get; internal set; }
        /// <summary>
        /// 如果块有方向约束，RestrictTo约束了只能从哪个方向出去
        /// </summary>
        public Direction RestrictTo { get; internal set; }
        /// <summary>
        /// 路线从哪个方向进入到本块中
        /// </summary>
        public Direction FromDirection { get; internal set; }
        /// <summary>
        /// 路线已经经过本块
        /// </summary>
        public bool IsPass { get; internal set; }
        /// <summary>
        /// 控制某个障碍的开关
        /// </summary>
        public int Switch { get; set; }
        /// <summary>
        /// 由某个开关控制
        /// </summary>
        public int BySwitch { get; set; }
    }
}
