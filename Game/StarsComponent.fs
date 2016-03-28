namespace Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type StarsComponent (game, graphics:GraphicsDeviceManager) =
    inherit DrawableGameComponent(game)
    // this constant controls the number of stars that will be created when the game
    // starts up.
    let [<Literal>] NumStars = 500

    // what percentage of those stars will be "big" stars? the default is 20%.
    let [<Literal>] PercentBigStars = 0.2

    // how bright will stars be?  somewhere between these two values.
    let [<Literal>] MinimumStarBrightness = 56
    let [<Literal>] MaximumStarBrightness = 255
    
    let batch = new PrimitiveBatch(graphics.GraphicsDevice)
    let stars = ResizeArray<Vector2>()
    let starColors = ResizeArray<Color>()

    let createStars () =
        // since every star will be put in a random place and have a random color, 
        // a random number generator might come in handy.
        let random = new Random()

        // where can we put the stars?      
        let screenWidth = graphics.GraphicsDevice.Viewport.Width
        let screenHeight = graphics.GraphicsDevice.Viewport.Height

        for i = 0 to NumStars-1 do      
            // pick a random spot...
            let rnd n = float32 <| random.Next(0,n)
            let where = Vector2(rnd screenWidth, rnd screenHeight)

            // ...and a random color. it's safe to cast random.Next to a byte,
            // because MinimumStarBrightness and MaximumStarBrightness are both
            // bytes.
            let greyValue = random.Next(MinimumStarBrightness, MaximumStarBrightness)
            let color = Color(greyValue, greyValue, greyValue)

            // if the random number was greater than the percentage chance for a big
            // star, this is just a normal star.
            if (random.NextDouble() > PercentBigStars)
            then
                starColors.Add(color)
                stars.Add(where)
            else        
                // if this star is randomly selected to be a "big" star, we actually
                // add four points and colors to stars and starColors. big stars are
                // a block of four points, instead of just one point.
                for j = 0 to 3 do
                    starColors.Add(color)

                stars.Add(where);
                stars.Add(where + Vector2.UnitX)
                stars.Add(where + Vector2.UnitY)
                stars.Add(where + Vector2.One)
            

    let drawStars() =
        // stars are drawn as a list of points, so begin the primitiveBatch.
        batch.Begin(PrimitiveType.TriangleList)

        // loop through all of the stars, and tell primitive batch to draw them.
        // each star is a very small triangle.
        for i = 0 to stars.Count-1 do
            let vector = stars.[i]
            let color = starColors.[i]
            batch.AddVertex(vector, color)
            batch.AddVertex(vector + Vector2.UnitX, color)
            batch.AddVertex(vector + Vector2.UnitY, color)
       
        // and then tell it that we're done.
        batch.End()

    do  createStars ()

    override this.Draw(gameTime) = drawStars()


