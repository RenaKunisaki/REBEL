# Functional bugs
- gravity blocks don't quite work right
    - they flip the screen but don't actually invert gravity
        - it's either this or you can't jump. I don't know how the hell the game manages it but it's a lot of ridiculous fragile nonsense I can't seem to reproduce.
    - hit detection is a bit wonky
        - probably all blocks' hit detection is wonky when flipped
- one-way gates might not work when you're going super fast
- the recipes are dumb because I haven't yet remembered to make them not dumb
- some things can be changed by hammering, others by right-click
    - this is a tModLoader limitation since hammering non-solid tiles doesn't work

# Visual bugs
- one-way gates cause jitter
    - actually acceptable for horizontal because it's like they're pushing you back (well, they are) but would be better if they worked like walls
    - for vertical it's awful
- the graphics are bad, someone please make better ones
    - especially some things are missing slope graphics
- the letter tiles all appear as A while being placed, but show up correctly once placed.
