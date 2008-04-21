xof 0303txt 0032


template VertexDuplicationIndices { 
 <b8d65549-d7c9-4995-89cf-53a9a8b031e3>
 DWORD nIndices;
 DWORD nOriginalVertices;
 array DWORD indices[nIndices];
}
template XSkinMeshHeader {
 <3cf169ce-ff7c-44ab-93c0-f78f62d172e2>
 WORD nMaxSkinWeightsPerVertex;
 WORD nMaxSkinWeightsPerFace;
 WORD nBones;
}
template SkinWeights {
 <6f0d123b-bad2-4167-a0d0-80224f25fabb>
 STRING transformNodeName;
 DWORD nWeights;
 array DWORD vertexIndices[nWeights];
 array float weights[nWeights];
 Matrix4x4 matrixOffset;
}

Frame RootFrame {

  FrameTransformMatrix {
    1.000000,0.000000,0.000000,0.000000,
    0.000000,0.000000,1.000000,0.000000,
    0.000000,1.000000,0.000000,0.000000,
    0.000000,0.000000,0.000000,1.000000;;
  }
Frame Plane_001 {

  FrameTransformMatrix {
    8.000000,0.000000,0.000000,0.000000,
    0.000000,0.000000,8.000000,0.000000,
    0.000000,-1.000000,0.000000,0.000000,
    0.000000,0.000000,0.000000,1.000000;;
  }
Mesh {
130;
1.000000; 4.000000; 41.000000;,
-1.000000; 4.000000; 41.000000;,
-1.000000; 0.000000; 41.000000;,
1.000000; 0.000000; 41.000000;,
-2.125000; 4.125000; 41.000000;,
-4.125000; 4.125000; 41.000000;,
-4.125000; 2.125000; 41.000000;,
-2.125000; 2.125000; 41.000000;,
4.125000; 4.125000; 41.000000;,
2.125000; 4.125000; 41.000000;,
2.125000; 2.125000; 41.000000;,
4.125000; 2.125000; 41.000000;,
5.000000; 5.500000; 48.000000;,
-4.987500; 5.500000; 48.000000;,
-4.987500; 5.000000; 48.000000;,
5.000000; 5.000000; 48.000000;,
-4.987500; 5.000000; 40.000000;,
5.000000; 5.000000; 40.000000;,
5.000000; 5.000000; 48.000000;,
-4.987500; 5.000000; 48.000000;,
-4.987500; 0.000000; 40.000000;,
5.000000; 0.000000; 40.000000;,
5.000000; 5.000000; 40.000000;,
-4.987500; 5.000000; 40.000000;,
-4.987500; 0.000000; -40.000000;,
5.000000; 0.000000; -40.000000;,
5.000000; 0.000000; 40.000000;,
-4.987500; 0.000000; 40.000000;,
-4.987500; 5.000000; -40.000000;,
5.000000; 5.000000; -40.000000;,
5.000000; 0.000000; -40.000000;,
-4.987500; 0.000000; -40.000000;,
-4.987500; 5.000000; -48.000000;,
5.000000; 5.000000; -48.000000;,
5.000000; 5.000000; -40.000000;,
-4.987500; 5.000000; -40.000000;,
-4.987500; 5.500000; -48.000000;,
5.000000; 5.500000; -48.000000;,
5.000000; 5.000000; -48.000000;,
-4.987500; 5.000000; -48.000000;,
-5.000000; 7.500000; 0.000000;,
5.000000; 7.500000; 0.000000;,
5.000000; 5.500000; -48.000000;,
-4.987500; 5.500000; -48.000000;,
-5.000000; 7.500000; 0.000000;,
-4.987500; 5.500000; 48.000000;,
5.000000; 5.500000; 48.000000;,
5.000000; 7.500000; 0.000000;,
-4.987500; 5.000000; 40.000000;,
-4.987500; 5.000000; -40.000000;,
-4.987500; 0.000000; -40.000000;,
-4.987500; 0.000000; 40.000000;,
-4.987500; 5.500000; 48.000000;,
-4.987500; 5.500000; -48.000000;,
-4.987500; 5.000000; -48.000000;,
-4.987500; 5.000000; 48.000000;,
-4.987500; 5.500000; 48.000000;,
-5.000000; 7.500000; 0.000000;,
-4.987500; 5.500000; -48.000000;,
5.000000; 5.500000; 48.000000;,
5.000000; 5.500000; -48.000000;,
5.000000; 7.500000; 0.000000;,
5.000000; 5.500000; 48.000000;,
5.000000; 5.000000; 48.000000;,
5.000000; 5.000000; -48.000000;,
5.000000; 5.500000; -48.000000;,
5.000000; 5.000000; 40.000000;,
5.000000; 0.000000; 40.000000;,
5.000000; 0.000000; -40.000000;,
5.000000; 5.000000; -40.000000;,
2.125000; 2.125000; 41.000000;,
2.125000; 4.125000; 41.000000;,
2.125000; 4.125000; 40.000000;,
2.125000; 2.125000; 40.000000;,
2.125000; 4.125000; 41.000000;,
4.125000; 4.125000; 41.000000;,
4.125000; 4.125000; 40.000000;,
2.125000; 4.125000; 40.000000;,
4.125000; 4.125000; 41.000000;,
4.125000; 2.125000; 41.000000;,
4.125000; 2.125000; 40.000000;,
4.125000; 4.125000; 40.000000;,
4.125000; 2.125000; 41.000000;,
2.125000; 2.125000; 41.000000;,
2.125000; 2.125000; 40.000000;,
4.125000; 2.125000; 40.000000;,
4.125000; 2.125000; 40.000000;,
2.125000; 2.125000; 40.000000;,
2.125000; 4.125000; 40.000000;,
4.125000; 4.125000; 40.000000;,
-1.000000; 0.000000; 41.000000;,
-1.000000; 4.000000; 41.000000;,
-1.000000; 4.000000; 40.000000;,
-1.000000; 0.000000; 40.000000;,
-1.000000; 4.000000; 41.000000;,
1.000000; 4.000000; 41.000000;,
1.000000; 4.000000; 40.000000;,
-1.000000; 4.000000; 40.000000;,
1.000000; 4.000000; 41.000000;,
1.000000; 0.000000; 41.000000;,
1.000000; 0.000000; 40.000000;,
1.000000; 4.000000; 40.000000;,
1.000000; 0.000000; 41.000000;,
-1.000000; 0.000000; 41.000000;,
-1.000000; 0.000000; 40.000000;,
1.000000; 0.000000; 40.000000;,
1.000000; 0.000000; 40.000000;,
-1.000000; 0.000000; 40.000000;,
-1.000000; 4.000000; 40.000000;,
1.000000; 4.000000; 40.000000;,
-4.125000; 2.125000; 41.000000;,
-4.125000; 4.125000; 41.000000;,
-4.125000; 4.125000; 40.000000;,
-4.125000; 2.125000; 40.000000;,
-4.125000; 4.125000; 41.000000;,
-2.125000; 4.125000; 41.000000;,
-2.125000; 4.125000; 40.000000;,
-4.125000; 4.125000; 40.000000;,
-2.125000; 4.125000; 41.000000;,
-2.125000; 2.125000; 41.000000;,
-2.125000; 2.125000; 40.000000;,
-2.125000; 4.125000; 40.000000;,
-2.125000; 2.125000; 41.000000;,
-4.125000; 2.125000; 41.000000;,
-4.125000; 2.125000; 40.000000;,
-2.125000; 2.125000; 40.000000;,
-2.125000; 2.125000; 40.000000;,
-4.125000; 2.125000; 40.000000;,
-4.125000; 4.125000; 40.000000;,
-2.125000; 4.125000; 40.000000;;
33;
4; 0, 3, 2, 1;,
4; 4, 7, 6, 5;,
4; 8, 11, 10, 9;,
4; 12, 15, 14, 13;,
4; 16, 19, 18, 17;,
4; 20, 23, 22, 21;,
4; 24, 27, 26, 25;,
4; 28, 31, 30, 29;,
4; 32, 35, 34, 33;,
4; 36, 39, 38, 37;,
4; 40, 43, 42, 41;,
4; 44, 47, 46, 45;,
4; 48, 51, 50, 49;,
4; 52, 55, 54, 53;,
3; 56, 58, 57;,
3; 59, 61, 60;,
4; 62, 65, 64, 63;,
4; 66, 69, 68, 67;,
4; 70, 73, 72, 71;,
4; 74, 77, 76, 75;,
4; 78, 81, 80, 79;,
4; 82, 85, 84, 83;,
4; 86, 89, 88, 87;,
4; 90, 93, 92, 91;,
4; 94, 97, 96, 95;,
4; 98, 101, 100, 99;,
4; 102, 105, 104, 103;,
4; 106, 109, 108, 107;,
4; 110, 113, 112, 111;,
4; 114, 117, 116, 115;,
4; 118, 121, 120, 119;,
4; 122, 125, 124, 123;,
4; 126, 129, 128, 127;;
  MeshMaterialList {
    4;
    33;
    1,
    0,
    0,
    3,
    3,
    2,
    2,
    2,
    3,
    3,
    3,
    3,
    2,
    3,
    3,
    3,
    3,
    2,
    0,
    0,
    0,
    0,
    0,
    1,
    1,
    1,
    1,
    1,
    0,
    0,
    0,
    0,
    0;;
  Material Material_002 {
    0.666667; 0.933333; 1.000000;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
  Material Material_003 {
    0.521569; 0.368627; 0.258824;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
  Material Material_004 {
    0.000000; 0.000000; 1.000000;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
  Material Material_005 {
    0.533333; 0.533333; 0.533333;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
    }  //End of MeshMaterialList
  MeshNormals {
130;
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000008; 1.000000;,
    0.000000; 0.000008; 1.000000;,
    0.000000; 0.000008; 1.000000;,
    0.000000; 0.000008; 1.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -0.000008; -1.000000;,
    0.000000; -0.000008; -1.000000;,
    0.000000; -0.000008; -1.000000;,
    0.000000; -0.000008; -1.000000;,
    0.000000; 0.999133; -0.041631;,
    0.000000; 0.999133; -0.041631;,
    0.000000; 0.999133; -0.041631;,
    0.000000; 0.999133; -0.041631;,
    0.000000; 0.999133; 0.041631;,
    0.000000; 0.999133; 0.041631;,
    0.000000; 0.999133; 0.041631;,
    0.000000; 0.999133; 0.041631;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -0.999980; -0.006250; 0.000000;,
    -0.999980; -0.006250; 0.000000;,
    -0.999980; -0.006250; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.000000; -1.000000;;
33;
4; 0, 3, 2, 1;,
4; 4, 7, 6, 5;,
4; 8, 11, 10, 9;,
4; 12, 15, 14, 13;,
4; 16, 19, 18, 17;,
4; 20, 23, 22, 21;,
4; 24, 27, 26, 25;,
4; 28, 31, 30, 29;,
4; 32, 35, 34, 33;,
4; 36, 39, 38, 37;,
4; 40, 43, 42, 41;,
4; 44, 47, 46, 45;,
4; 48, 51, 50, 49;,
4; 52, 55, 54, 53;,
3; 56, 58, 57;,
3; 59, 61, 60;,
4; 62, 65, 64, 63;,
4; 66, 69, 68, 67;,
4; 70, 73, 72, 71;,
4; 74, 77, 76, 75;,
4; 78, 81, 80, 79;,
4; 82, 85, 84, 83;,
4; 86, 89, 88, 87;,
4; 90, 93, 92, 91;,
4; 94, 97, 96, 95;,
4; 98, 101, 100, 99;,
4; 102, 105, 104, 103;,
4; 106, 109, 108, 107;,
4; 110, 113, 112, 111;,
4; 114, 117, 116, 115;,
4; 118, 121, 120, 119;,
4; 122, 125, 124, 123;,
4; 126, 129, 128, 127;;
}  //End of MeshNormals
MeshTextureCoords {
130;
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
1.000000;-1.000000;,
1.000000;0.000000;,
0.000000;0.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;,
0.000000;-1.000000;,
0.000000;0.000000;,
1.000000;0.000000;,
1.000000;-1.000000;;
}  //End of MeshTextureCoords
 }
}
}
