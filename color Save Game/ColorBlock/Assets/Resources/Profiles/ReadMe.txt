These are Objects and store the data during runtime they are currently set up to auto save when the data is changed
if you want to turn off auto save take the "saveGameAction" item off of the "GameData" object in the scene.
the data in these objects saves to a Json and reads and writes from this json so the data persists between sessions.