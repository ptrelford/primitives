namespace Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

/// <summary>
/// A drawable game component that shows a rotating text.
/// </summary>
type RotatingTextComponent(game, spriteBatch : SpriteBatch, font : SpriteFont) =  
    inherit DrawableGameComponent(game)
    
    let mutable theta = 0.0f

    override this.Update gt =
        theta <- (theta + float32 gt.ElapsedGameTime.TotalSeconds) % MathHelper.TwoPi
        base.Update gt
    
    override this.Draw gt =
        spriteBatch.Begin()
        spriteBatch.DrawString(font, "Hello XNA+F# World!", Vector2(50.0f,50.0f), Color.Yellow, theta, Vector2.Zero, 1.0f, 
            SpriteEffects.None, 0.0f)
        spriteBatch.End()    
        base.Draw gt

