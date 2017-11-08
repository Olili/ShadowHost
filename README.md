ShadowHost


Coucou.Tout est en WIP.
	
	Description des scenes : 
Dans Scene1
Il y a une araignée joueur. et des araignée suiveuses de souris.

Dans test2
Contient un HordeManager

Si votre scene contient un HordeManager
vous pouvez invoquer une horde avec le click droit. 
On peut changer le nb de personne invoquée dans le prefab
La horde est remplie de follower et d'un player Alpha.


Pour avoir une araignée en faisant tout soi meme : Poser le prefab d'araignée puis ajouter un brain
	--> Player_Brain : Unité controllable. 
	--> followBrain : va planter. ( sauf si vous lui donner un alpha en référence)
	--> MouseBrain : suit la souris. 
