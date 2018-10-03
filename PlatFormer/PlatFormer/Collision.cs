using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatFormer
{
    class Collision
    {
        public Game1 game;

        public bool IsColliding(Sprite hero, Sprite otherSprite)
        {
            // Compare the position of each rectangles edges against the other
            // It compares opposite edges of edges of each rectangles, ie, the left edge of one 
            if (hero.rightEdge < otherSprite.leftEdge || hero.leftEdge > otherSprite.rightEdge || hero.bottomEdge < otherSprite.bottomEdge || hero.topEdge > otherSprite.bottomEdge)
            {
                //these two rectangles are not colliding
                return false;
            }
            //otherwise, the two AABB rectangles overlap, therefore there is a collision
            return true;
        }

        public Sprite CollideWithPlayer(Sprite hero, float deltaTime)
        {
            // create a copy of the hero that will move to where the hero will be in the next frame.
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position
        }
    }
}
