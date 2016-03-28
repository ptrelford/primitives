module Game.Ship

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

// how big is the ship
let [<Literal>] ShipSizeX = 10.0f
let [<Literal>] ShipSizeY = 15.0f
let [<Literal>] ShipCutoutSize = 5.0f

// called to draw the spacewars ship at a point on the screen.
let draw(batch:PrimitiveBatch, where:Vector2) =
    let color = Color.White
    
    // tell the primitive batch to start drawing lines
    batch.Begin(PrimitiveType.LineList)

    // from the nose, down the left hand side
    batch.AddVertex(where + Vector2(0.0f, -ShipSizeY), color)
    batch.AddVertex(where + Vector2(-ShipSizeX, ShipSizeY), color)

    // to the right and up, into the cutout
    batch.AddVertex(where + Vector2(-ShipSizeX, ShipSizeY), color)
    batch.AddVertex(where + Vector2(0.0f, ShipSizeY - ShipCutoutSize), color)

    // to the right and down, out of the cutout
    batch.AddVertex(where + Vector2(0.0f, ShipSizeY - ShipCutoutSize), color)
    batch.AddVertex(where + Vector2(ShipSizeX, ShipSizeY), color)

    // and back up to the nose, where we started.
    batch.AddVertex(where + Vector2(ShipSizeX, ShipSizeY), color)
    batch.AddVertex(where + Vector2(0.0f, -ShipSizeY), color)

    // and we're done.
    batch.End()


