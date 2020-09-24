using OutOfHome.Models.Views;
using System;

namespace OutOfHome.Exports.GoogleMaps
{
    public sealed class ColoredBoard : BaseBoardModelView, IColored
    {
        public System.Drawing.Color Color { get; set; }
        public ColoredBoard(BaseBoardModelView board) : base(board)
        { 
        
        }
        
    }
}
