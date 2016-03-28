module Game.Sun

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

// the radius of the sun.
let [<Literal>] SunSize = 30.0f

// called to draw the spacewars sun.
let draw (primitiveBatch:PrimitiveBatch, where:Vector2) =
    // the sun is made from 4 lines in a circle.
    primitiveBatch.Begin(PrimitiveType.LineList)

    // draw the vertical and horizontal lines
    primitiveBatch.AddVertex(where + Vector2(0.0f, SunSize), Color.White)
    primitiveBatch.AddVertex(where + Vector2(0.0f, -SunSize), Color.White)

    primitiveBatch.AddVertex(where + Vector2(SunSize, 0.0f), Color.White)
    primitiveBatch.AddVertex(where + Vector2(-SunSize, 0.0f), Color.White)

    // to know where to draw the diagonal lines, we need to use trig.
    // cosine of pi / 4 tells us what the x coordinate of a circle's radius is
    // at 45 degrees. the y coordinate normally would come from sin, but sin and
    // cos 45 are the same, so we can reuse cos for both x and y.
    let sunSizeDiagonal = (cos MathHelper.PiOver4) * SunSize

    // since that trig tells us the x and y for a unit circle, which has a
    // radius of 1, we need scale that result by the sun's radius.
    //sunSizeDiagonal *= SunSize;

    primitiveBatch.AddVertex(
        where + Vector2(-sunSizeDiagonal, sunSizeDiagonal), Color.Gray)
    primitiveBatch.AddVertex(
        where + Vector2(sunSizeDiagonal, -sunSizeDiagonal), Color.Gray)

    primitiveBatch.AddVertex(
        where + Vector2(sunSizeDiagonal, sunSizeDiagonal), Color.Gray)
    primitiveBatch.AddVertex(
        where + Vector2(-sunSizeDiagonal, -sunSizeDiagonal), Color.Gray)

    primitiveBatch.End()

