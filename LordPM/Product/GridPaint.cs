using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace LordJiraManager.Product
{
   public class GridPaint : GridStringCell
    {
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            RECT textRect = new RECT(rect.left + 10, rect.top + 2, rect.right - 10, rect.bottom - 2);
            paint.FillRoundRect(CDraw.PCOLORS_BACKCOLOR, textRect, 3);
            String text = Text;
            FONT font = new FONT();
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT();
            int width = textRect.right - textRect.left;
            int height = textRect.bottom - textRect.top;
            tRect.left = textRect.left + (width - tSize.cx) / 2;
            tRect.top = textRect.top + (height - tSize.cy) / 2;
            tRect.right = tRect.left + tSize.cx;
            tRect.bottom = tRect.top + tSize.cy;
            paint.DrawText(text, COLOR.ARGB(255, 255, 255), font, tRect);
        }
        
    }
}
