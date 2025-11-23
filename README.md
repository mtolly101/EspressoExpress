# Espresso Express

A simple 2D coffee making game built in Unity where you fulfill coffee orders by adding ingredients to your cup in the correct order before the timer runs out.

## How to Play
- Complete as many "Latte" orders as you can before time runs out
- The current order is shown at the top of the screen which is "Order: Latte (Espresso + Milk)"
- Click ingredient buttons at the bottom of the screen to add them to your cup:
  - Espresso
  - Water
  - Milk
  - Cocoa
  - Sugar
- The center of the screen shows your cup contents which goes as follows "Cup: Espresso + Milk"
- When you’re ready click Serve:
  - If the cup matches the recipe then you gain 1 point for the score
  - If it doesn’t match the cup is cleared and there is no score for that attempt
- The timer counts down from 60 seconds and when it reaches 0 the Game Over screen appears
- When the Game Over screen appears click the Restart to play again
- Click the ingredient buttons to add to your cup
- Match the order shown on the left in exact sequence
- For the correct order add 1 to the score and add 5 seconds bonus time
- For the wrong ingredient your cup resets
- Run out of time and the Game Over screen comes up so click Restart

## Controls
- Mouse click only

## Current Build
- 2D UI based layout with:
  - Top bar: Score, Timer, current Order
  - Middle: Cup panel with coffee cup sprite and cup contents text
  - Bottom bar: 5 ingredient buttons and Serve button
- Working game loop:
  - Timer counts down from 60 seconds
  - Clicking ingredient buttons updates the cup contents
  - Serve button checks if cup matches the recipe of "Espresso + Milk"
  - Score increases on correct serve
  - Cup is cleared after every serve attempt whether it's correct or incorrect
  - Game Over panel appears when time reaches 0 with a Restart button
- WebGL build that runs on Unity Play

## Issues
- Only one recipe is implemented "Latte: Espresso + Milk"
- No sound effects or music yet
- No randomization of orders or difficulty scaling
- Visuals are simple with minimal animation
- Incorrect orders only clear the cup and they do not have additional penalties

## Improvements For Next Milestone
- Add multiple recipes and random order selection
- Add time bonuses or penalties based on correct/incorrect serves
- Add simple sound effects for clicks, success, and failure
- Add visual feedback when an order is correct or wrong
- Add more recipes and maybe a “day summary” screen after the timer ends

## Links
- Unity Play: https://play.unity.com/en/games/f66ce4b7-12b4-402c-97cb-8dc7f6d548eb/coffee-clicker-beta-v01
