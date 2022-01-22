# Functional bugs
- one-way gates might not work when you're going super fast
- some things can be changed by hammering, others by right-click
    - this is a tModLoader limitation since hammering non-solid tiles doesn't work
- several things check if a tile is within the world but mistakenly assume Main.topWorld, etc are in tile coords (they're not)
    - should just use Terraria.Framing.getTileSafely

# Visual bugs
- one-way gates cause jitter
    - actually acceptable for horizontal because it's like they're pushing you back (well, they are) but would be better if they worked like walls
    - for vertical it's awful
- the graphics are bad, someone please make better ones
    - especially some things are missing slope graphics
- the letter tiles and Numeric Display Digit all appear as A while being placed, but show up correctly once placed.
    - I'm sure this is trivial to fix but nobody can tell me how
