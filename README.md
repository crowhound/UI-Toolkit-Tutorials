### Overview
This is the repo for the public UI Toolkit Tutorials I have shown on the ShatterFantasyStudio YouTube channel. The repo will start off small and quickly grow when I bring the SF UI Elements stuff into it for people to see how to use them.

Link to Channel
https://www.youtube.com/@ShatterFantasyStudio



### Important requirements and Information:
The following information is common between all my public tutorial repos and demo packages unless stated otherwise in that specific repo or package.

Currently Unity 6000.5.0f1 is the minimum supported version. There will be git branches for newer versions as well to show off newer features and API.
All my repos and projects have domain reload turned off for future CoreCLR support. I won't use legacy API that have newer versions, as long as they are actually improved API.
This is to prevent technical debt and to make sure all tutorials show the proper modern way to do stuff in Unity. I don't want to teach people to use obsolete or deprecated tools.

Also removing them vastly increase build times and performance.

This means my tutorials have none of the following:
- The Old Input Manager - I only use the Input System package.
- No UGUI/Text Mesh Pro - I removed it from my projects.
  - Note any multiplayer tutorial project before Unity 6.6 will have a UGUI reference because Unity.Authentication package for some reason has a hard dependacy on it.
- Any 2D physics demos will use the Physics Core 2D module - I have not used Collider2D or Rigidbody2D in any project since Unity 6.3 I only use PhysicsBody and PhysicsShape for performance and customization.
  - Some of Unity's internal stuff might have a reference to the built in module. If not I have the legacyy 2D physics Module disabled so even Rigibdoy and Colliders don't appear as components.
- No BIRP OR HDRP - Only using URP since it is the main SRP going forward by Unity.



### Granted permissions and reserved rights
Do not use anything to train any AI models. I reserve the right to disable and block all public repo access, if it is discovered people are training or scraping any code with AI.

Other than don't use any repo resource for AI, pretty much use it how you want. You can use it for learning, if you like any of the Style sheets, themes or custom visual elements you can use them for your project. 
You don't have to give credit and you can use anything for commercial use.
