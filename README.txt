----------------------
Contents of the Folder
----------------------
1. Unity Project (Library folder not included due to large size)
2. Video of the Arthrex Assignment app running on Oneplus Nord 2
3. .apk file of the project
4. Image used for Image Tracking (Doodle.jpg)
5. Readme file



---------
Approach
--------- 

The approach for implementing the assignment majorly consist of three parts:
1. Creating an Image Tracking AR app - Used basic image tracking setup provided by ARFoundation
2. User Input Handling for particle motion
	-> Created a 50x50 grid texture (configurable from code based on device performance) for handling user inputs
	-> Texture contains the current offset of the particle from its original position
	-> Updating the texture per frame based on impluse velocity & acceleration(when touch detected) and a constant deacceleration
	-> Uploading the texture to the GPU to be used by renderer to update the particles' positions.
3. Rendering of Particles
	-> Created a procedural mesh (Grid size : 100x100,  Total vertices: 4*Gridsize "One Quad per grid cell", Grid size configurable from code)
	-> Each grid cell is basically a quad responsible for rendering one particle.
	-> Created an unlit shader which takes each grid cell and renders the particle in a circular shape (implemented in fragment shader).
	-> Each particles' color is sampled from the Input texture.
	-> Texture from Step 2 is set as 'Motion Tex' in the shader and is used for sampling the position offset in vertex  shader.
	-> The position offset sampled is added to all the vertices of the corresponding Quad.
	-> Set the shader to the material used by the procedural mesh for rendering.


 