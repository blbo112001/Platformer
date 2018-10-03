using Microsoft.Xna.Framework;
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

        bool CheckForTile( Vector2 coordinates) // Checks if there is a tile at the specified coordinates.
        {
            int column = (int)coordinates.X;
            int row = (int)coordinates.Y;

            if (column < 0 || column > game.levelTileWidth - 1)
            {
                return false;
            }
            if (row < 0 || row > game.LevelTileHeight - 1)
            {
                return true;
            }

            Sprite tileFound = game.levelGrid[column, row];

            if (tileFound != null)
            {
                return true;
            }

            return false;
        }

        Sprite CollideLeft(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X < 0)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;

            }
            return hero;
        }

        Sprite CollideRight(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPrediction, tile) == true && hero.velocity.X > 0)
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                hero.velocity.X = 0;
            }

            return hero;
        }

        Sprite CollideAbove(Sprite hero, Vector2 tileIndex, Sprite playerPredictions)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];

            if (IsColliding(playerPredictions, tile) == true && hero.velocity.Y < 0)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }

            return hero;
        }

        Sprite collideBelow(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            if (IsColliding(playerPrediction, tile) == true && hero.velocity.Y > 0)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }
            return hero;
        }


        Sprite CollideBottomDiagonals(Sprite hero, Vector2 TileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)TileIndex.X, (int)TileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int rightEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int topEdgeDistance = Math.Abs(tile.topEdge - playerPrediction.topEdge);

            if (IsColliding(playerPrediction, tile) == true)
            {
                if (topEdgeDistance < rightEdgeDistance && topEdgeDistance < leftEdgeDistance)
                {
                    // If the top edge is closest, collision is happening to above the platform
                    hero.position.Y = tile.topEdge - hero.height + hero.offset.Y;
                    hero.velocity.Y = 0;
                }
                else if (rightEdgeDistance < leftEdgeDistance)
                {
                    // If the right edge is closest, collision is happening to the right side of the platform
                    hero.position.X = tile.rightEdge + hero.offset.X;
                    hero.velocity.Y = 0;
                }
                else
                {
                    // else if the left edge is closest, the collision is happening to the left of the platform
                    hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                    hero.velocity.X = 0;
                }
            }
            return hero;
        }

        Sprite CollideAboveDiagonals(Sprite hero, Vector2 tileIndex, Sprite playerPrediction)
        {
            Sprite tile = game.levelGrid[(int)tileIndex.X, (int)tileIndex.Y];
            int leftEdgeDistance = Math.Abs(tile.rightEdge - playerPrediction.leftEdge);
            int rightEdgeDistance = Math.Abs(tile.leftEdge - playerPrediction.rightEdge);
            int bottomEdgeDistance = Math.Abs(tile.bottomEdge - playerPrediction.bottomEdge);
            if (IsColliding(playerPrediction, tile) == true)
            {
                hero.position.Y = tile.bottomEdge + hero.offset.Y;
                hero.velocity.Y = 0;
            }
            else if (leftEdgeDistance < rightEdgeDistance)
            {
                hero.position.X = tile.rightEdge + hero.offset.X;
                hero.velocity.X = 0;
            }
            else
            {
                hero.position.X = tile.leftEdge - hero.width + hero.offset.X;
                hero.velocity.X = 0;
            }
            return hero;
        }

        public Sprite CollideWithPlatforms(Sprite hero, float deltaTime)
        {
            // create a copy of the hero that will move to where the hero will be in the next frame.
            Sprite playerPrediction = new Sprite();
            playerPrediction.position = hero.position;
            playerPrediction.width = hero.width;
            playerPrediction.height = hero.height;
            playerPrediction.offset = hero.offset;
            playerPrediction.UpdateHitBox();

            playerPrediction.position += hero.velocity * deltaTime;

            int playerColumn = (int)Math.Round(playerPrediction.position.X / game.tileHeight);
            int playerRow = (int)Math.Round(playerPrediction.position.Y / game.tileHeight);

            Vector2 playerTile = new Vector2(playerColumn, playerRow);

            Vector2 leftTile = new Vector2(playerTile.X - 1, playerTile.Y);
            Vector2 rightTile = new Vector2(playerTile.X + 1, playerTile.Y);
            Vector2 topTile = new Vector2(playerTile.X, playerTile.Y - 1);
            Vector2 bottomTile = new Vector2(playerTile.X, playerTile.Y + 1);

            Vector2 bottomLeftTile = new Vector2(playerTile.X - 1, playerTile.Y + 1);
            Vector2 bottomRightTile = new Vector2(playerTile.X + 1, playerTile.Y + 1);
            Vector2 topLeftTile = new Vector2(playerTile.X - 1, playerTile.Y - 1);
            Vector2 topRightTile = new Vector2(playerTile.X + 1, playerTile.Y - 1);
            //... This allows us to predict if the hero wil be pverlapping an obstacle in the next frame

            bool leftCheck = CheckForTile(leftTile);
            bool rightCheck = CheckForTile(rightTile);
            bool topCheck = CheckForTile(topTile);
            bool bottomCheck = CheckForTile(bottomTile);

            bool bottomLeftCheck = CheckForTile(bottomLeftTile);
            bool bottomRIghtCheck = CheckForTile(bottomRightTile);
            bool topLeftCheck = CheckForTile(topLeftTile);
            bool topRIghtCheck = CheckForTile(topRightTile);

            if (leftCheck == true) // Check for collisions with the tiles left of the player
            {
                hero = CollideLeft(hero, leftTile, playerPrediction);
            }

            if (rightCheck == true)
            {
                hero = CollideRight(hero, rightTile, playerPrediction);
            }
            if (bottomCheck == true)
            {
                hero = collideBelow(hero, bottomTile, playerPrediction);
            }
            if (topCheck == true)
            {
                hero = CollideAbove(hero, topTile, playerPrediction);
            }
            return hero;
        }
    }
}
