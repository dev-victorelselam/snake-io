Hello! this is Victor and I'll write some observations here

1 - First, I would like to highlight the custom Unit Test Framework, developed by me during this test and can be found at: https://github.com/dev-victorelselam/unity-runtime-test
It works with Unity scenes, so it could be run inside mobile builds. I plan to extend it to run automatically, collect results and possibly send it to a custom server. 
Also, soon it should be available in Unity Package Manager, I'm currently making the setup for it.

2 - I didn't test this framework UX, but it's very easy to use. Just open the selected Test Scene (located at Assets/_Tests/{folder}) and click Play.

3 - Unfortunately, I don't know how to generate procedural meshs so I couldn't complete this task, that in my opinion, is the hardest challenge in this test.

4 - I chose to build a Tutorial UI instead of just pressing keys while game run, however, you asked for it, so I added a key detector in game running to trigger the Tutorial UI. 
I planned to assign the Keys automatically in this scenario, but this would require a 'Chain of Responsability' pattern in the tutorial flow and I couldn't do it in time :( 

5 - The architecture diagram is located in Assets/


