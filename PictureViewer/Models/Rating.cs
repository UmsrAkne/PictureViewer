namespace PictureViewer.Models
{
    public enum Rating
    {
        /// <summary>
        /// 未評価であることを表します。
        /// </summary>
        NoRating = 0,

        /// <summary>
        /// 最も良い評価を表します。
        /// </summary>
        A = 1,

        /// <summary>
        /// A に次ぐ評価を表します。
        /// </summary>
        B = 2,

        /// <summary>
        /// B に次ぐ評価を表します。
        /// </summary>
        C = 3,

        /// <summary>
        /// C に次ぐ評価を表します。
        /// </summary>
        D = 4,

        /// <summary>
        /// 最低評価を表します。
        /// </summary>
        E = 5,
    }
}