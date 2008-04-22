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
    0.000000,0.000000,-1.000000,0.000000,
    0.000000,1.000000,0.000000,0.000000,
    0.000000,0.000000,0.000000,1.000000;;
  }
  Frame Sphere {

    FrameTransformMatrix {
      1.000000,0.000000,0.000000,0.000000,
      0.000000,1.000000,0.000000,0.000000,
      0.000000,0.000000,0.250000,0.000000,
      0.000000,0.000000,0.000000,1.000000;;
    }
Mesh {
224;
0.000000; 0.000000; -1.000000;,
0.653300; 0.270600; -0.707100;,
0.707100; 0.000000; -0.707100;,
0.707100; 0.000000; -0.707100;,
0.653300; 0.270600; -0.707100;,
0.923900; 0.382700; 0.000000;,
1.000000; 0.000000; 0.000000;,
1.000000; 0.000000; 0.000000;,
0.923900; 0.382700; 0.000000;,
0.653300; 0.270600; 0.707100;,
0.707100; 0.000000; 0.707100;,
0.707100; 0.000000; 0.707100;,
0.653300; 0.270600; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.653300; 0.270600; 0.707100;,
0.500000; 0.500000; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.923900; 0.382700; 0.000000;,
0.707100; 0.707100; 0.000000;,
0.500000; 0.500000; 0.707100;,
0.653300; 0.270600; 0.707100;,
0.653300; 0.270600; -0.707100;,
0.500000; 0.500000; -0.707100;,
0.707100; 0.707100; 0.000000;,
0.923900; 0.382700; 0.000000;,
0.000000; 0.000000; -1.000000;,
0.500000; 0.500000; -0.707100;,
0.653300; 0.270600; -0.707100;,
0.000000; 0.000000; -1.000000;,
0.270600; 0.653300; -0.707100;,
0.500000; 0.500000; -0.707100;,
0.500000; 0.500000; -0.707100;,
0.270600; 0.653300; -0.707100;,
0.382700; 0.923900; 0.000000;,
0.707100; 0.707100; 0.000000;,
0.707100; 0.707100; 0.000000;,
0.382700; 0.923900; 0.000000;,
0.270600; 0.653300; 0.707100;,
0.500000; 0.500000; 0.707100;,
0.500000; 0.500000; 0.707100;,
0.270600; 0.653300; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.270600; 0.653300; 0.707100;,
0.000000; 0.707100; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.382700; 0.923900; 0.000000;,
0.000000; 1.000000; 0.000000;,
0.000000; 0.707100; 0.707100;,
0.270600; 0.653300; 0.707100;,
0.270600; 0.653300; -0.707100;,
0.000000; 0.707100; -0.707100;,
0.000000; 1.000000; 0.000000;,
0.382700; 0.923900; 0.000000;,
0.000000; 0.000000; -1.000000;,
0.000000; 0.707100; -0.707100;,
0.270600; 0.653300; -0.707100;,
0.000000; 0.000000; -1.000000;,
-0.270600; 0.653300; -0.707100;,
0.000000; 0.707100; -0.707100;,
0.000000; 0.707100; -0.707100;,
-0.270600; 0.653300; -0.707100;,
-0.382700; 0.923900; 0.000000;,
0.000000; 1.000000; 0.000000;,
0.000000; 1.000000; 0.000000;,
-0.382700; 0.923900; 0.000000;,
-0.270600; 0.653300; 0.707100;,
0.000000; 0.707100; 0.707100;,
0.000000; 0.707100; 0.707100;,
-0.270600; 0.653300; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.270600; 0.653300; 0.707100;,
-0.500000; 0.500000; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.382700; 0.923900; 0.000000;,
-0.707100; 0.707100; 0.000000;,
-0.500000; 0.500000; 0.707100;,
-0.270600; 0.653300; 0.707100;,
-0.270600; 0.653300; -0.707100;,
-0.500000; 0.500000; -0.707100;,
-0.707100; 0.707100; 0.000000;,
-0.382700; 0.923900; 0.000000;,
0.000000; 0.000000; -1.000000;,
-0.500000; 0.500000; -0.707100;,
-0.270600; 0.653300; -0.707100;,
0.000000; 0.000000; -1.000000;,
-0.653300; 0.270600; -0.707100;,
-0.500000; 0.500000; -0.707100;,
-0.500000; 0.500000; -0.707100;,
-0.653300; 0.270600; -0.707100;,
-0.923900; 0.382700; 0.000000;,
-0.707100; 0.707100; 0.000000;,
-0.707100; 0.707100; 0.000000;,
-0.923900; 0.382700; 0.000000;,
-0.653300; 0.270600; 0.707100;,
-0.500000; 0.500000; 0.707100;,
-0.500000; 0.500000; 0.707100;,
-0.653300; 0.270600; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.653300; 0.270600; 0.707100;,
-0.707100; 0.000000; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.923900; 0.382700; 0.000000;,
-1.000000; 0.000000; 0.000000;,
-0.707100; 0.000000; 0.707100;,
-0.653300; 0.270600; 0.707100;,
-0.653300; 0.270600; -0.707100;,
-0.707100; 0.000000; -0.707100;,
-1.000000; 0.000000; 0.000000;,
-0.923900; 0.382700; 0.000000;,
0.000000; 0.000000; -1.000000;,
-0.707100; 0.000000; -0.707100;,
-0.653300; 0.270600; -0.707100;,
0.000000; 0.000000; -1.000000;,
-0.653300; -0.270600; -0.707100;,
-0.707100; 0.000000; -0.707100;,
-0.707100; 0.000000; -0.707100;,
-0.653300; -0.270600; -0.707100;,
-0.923900; -0.382700; 0.000000;,
-1.000000; 0.000000; 0.000000;,
-1.000000; 0.000000; 0.000000;,
-0.923900; -0.382700; 0.000000;,
-0.653300; -0.270600; 0.707100;,
-0.707100; 0.000000; 0.707100;,
-0.707100; 0.000000; 0.707100;,
-0.653300; -0.270600; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.653300; -0.270600; 0.707100;,
-0.500000; -0.500000; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.923900; -0.382700; 0.000000;,
-0.707100; -0.707100; 0.000000;,
-0.500000; -0.500000; 0.707100;,
-0.653300; -0.270600; 0.707100;,
-0.653300; -0.270600; -0.707100;,
-0.500000; -0.500000; -0.707100;,
-0.707100; -0.707100; 0.000000;,
-0.923900; -0.382700; 0.000000;,
0.000000; 0.000000; -1.000000;,
-0.500000; -0.500000; -0.707100;,
-0.653300; -0.270600; -0.707100;,
0.000000; 0.000000; -1.000000;,
-0.270600; -0.653300; -0.707100;,
-0.500000; -0.500000; -0.707100;,
-0.500000; -0.500000; -0.707100;,
-0.270600; -0.653300; -0.707100;,
-0.382700; -0.923900; 0.000000;,
-0.707100; -0.707100; 0.000000;,
-0.707100; -0.707100; 0.000000;,
-0.382700; -0.923900; 0.000000;,
-0.270600; -0.653300; 0.707100;,
-0.500000; -0.500000; 0.707100;,
-0.500000; -0.500000; 0.707100;,
-0.270600; -0.653300; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.270600; -0.653300; 0.707100;,
0.000000; -0.707100; 0.707100;,
0.000000; 0.000000; 1.000000;,
-0.382700; -0.923900; 0.000000;,
0.000000; -1.000000; 0.000000;,
0.000000; -0.707100; 0.707100;,
-0.270600; -0.653300; 0.707100;,
-0.270600; -0.653300; -0.707100;,
0.000000; -0.707100; -0.707100;,
0.000000; -1.000000; 0.000000;,
-0.382700; -0.923900; 0.000000;,
0.000000; 0.000000; -1.000000;,
0.000000; -0.707100; -0.707100;,
-0.270600; -0.653300; -0.707100;,
0.000000; 0.000000; -1.000000;,
0.270600; -0.653300; -0.707100;,
0.000000; -0.707100; -0.707100;,
0.000000; -0.707100; -0.707100;,
0.270600; -0.653300; -0.707100;,
0.382700; -0.923900; 0.000000;,
0.000000; -1.000000; 0.000000;,
0.000000; -1.000000; 0.000000;,
0.382700; -0.923900; 0.000000;,
0.270600; -0.653300; 0.707100;,
0.000000; -0.707100; 0.707100;,
0.000000; -0.707100; 0.707100;,
0.270600; -0.653300; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.270600; -0.653300; 0.707100;,
0.500000; -0.500000; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.382700; -0.923900; 0.000000;,
0.707100; -0.707100; 0.000000;,
0.500000; -0.500000; 0.707100;,
0.270600; -0.653300; 0.707100;,
0.270600; -0.653300; -0.707100;,
0.500000; -0.500000; -0.707100;,
0.707100; -0.707100; 0.000000;,
0.382700; -0.923900; 0.000000;,
0.000000; 0.000000; -1.000000;,
0.500000; -0.500000; -0.707100;,
0.270600; -0.653300; -0.707100;,
0.000000; 0.000000; -1.000000;,
0.653300; -0.270600; -0.707100;,
0.500000; -0.500000; -0.707100;,
0.500000; -0.500000; -0.707100;,
0.653300; -0.270600; -0.707100;,
0.923900; -0.382700; 0.000000;,
0.707100; -0.707100; 0.000000;,
0.707100; -0.707100; 0.000000;,
0.923900; -0.382700; 0.000000;,
0.653300; -0.270600; 0.707100;,
0.500000; -0.500000; 0.707100;,
0.500000; -0.500000; 0.707100;,
0.653300; -0.270600; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.653300; -0.270600; 0.707100;,
0.707100; 0.000000; 0.707100;,
0.000000; 0.000000; 1.000000;,
0.923900; -0.382700; 0.000000;,
1.000000; 0.000000; 0.000000;,
0.707100; 0.000000; 0.707100;,
0.653300; -0.270600; 0.707100;,
0.653300; -0.270600; -0.707100;,
0.707100; 0.000000; -0.707100;,
1.000000; 0.000000; 0.000000;,
0.923900; -0.382700; 0.000000;,
0.000000; 0.000000; -1.000000;,
0.707100; 0.000000; -0.707100;,
0.653300; -0.270600; -0.707100;;
64;
3; 0, 1, 2;,
4; 3, 4, 5, 6;,
4; 7, 8, 9, 10;,
3; 11, 12, 13;,
3; 14, 15, 16;,
4; 17, 18, 19, 20;,
4; 21, 22, 23, 24;,
3; 25, 26, 27;,
3; 28, 29, 30;,
4; 31, 32, 33, 34;,
4; 35, 36, 37, 38;,
3; 39, 40, 41;,
3; 42, 43, 44;,
4; 45, 46, 47, 48;,
4; 49, 50, 51, 52;,
3; 53, 54, 55;,
3; 56, 57, 58;,
4; 59, 60, 61, 62;,
4; 63, 64, 65, 66;,
3; 67, 68, 69;,
3; 70, 71, 72;,
4; 73, 74, 75, 76;,
4; 77, 78, 79, 80;,
3; 81, 82, 83;,
3; 84, 85, 86;,
4; 87, 88, 89, 90;,
4; 91, 92, 93, 94;,
3; 95, 96, 97;,
3; 98, 99, 100;,
4; 101, 102, 103, 104;,
4; 105, 106, 107, 108;,
3; 109, 110, 111;,
3; 112, 113, 114;,
4; 115, 116, 117, 118;,
4; 119, 120, 121, 122;,
3; 123, 124, 125;,
3; 126, 127, 128;,
4; 129, 130, 131, 132;,
4; 133, 134, 135, 136;,
3; 137, 138, 139;,
3; 140, 141, 142;,
4; 143, 144, 145, 146;,
4; 147, 148, 149, 150;,
3; 151, 152, 153;,
3; 154, 155, 156;,
4; 157, 158, 159, 160;,
4; 161, 162, 163, 164;,
3; 165, 166, 167;,
3; 168, 169, 170;,
4; 171, 172, 173, 174;,
4; 175, 176, 177, 178;,
3; 179, 180, 181;,
3; 182, 183, 184;,
4; 185, 186, 187, 188;,
4; 189, 190, 191, 192;,
3; 193, 194, 195;,
3; 196, 197, 198;,
4; 199, 200, 201, 202;,
4; 203, 204, 205, 206;,
3; 207, 208, 209;,
3; 210, 211, 212;,
4; 213, 214, 215, 216;,
4; 217, 218, 219, 220;,
3; 221, 222, 223;;
  MeshMaterialList {
    1;
    64;
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0;;
  Material Material {
    0.236318; 0.336528; 1.000000;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
    }  //End of MeshMaterialList
  MeshNormals {
224;
    0.000000; 0.000000; -1.000000;,
    0.651418; 0.269814; -0.709098;,
    0.705069; 0.000000; -0.709098;,
    0.705069; 0.000000; -0.709098;,
    0.651418; 0.269814; -0.709098;,
    0.923856; 0.382672; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    0.923856; 0.382672; 0.000000;,
    0.651418; 0.269814; 0.709098;,
    0.705069; 0.000000; 0.709098;,
    0.705069; 0.000000; 0.709098;,
    0.651418; 0.269814; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.651418; 0.269814; 0.709098;,
    0.498550; 0.498550; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.923856; 0.382672; 0.000000;,
    0.707083; 0.707083; 0.000000;,
    0.498550; 0.498550; 0.709098;,
    0.651418; 0.269814; 0.709098;,
    0.651418; 0.269814; -0.709098;,
    0.498550; 0.498550; -0.709098;,
    0.707083; 0.707083; 0.000000;,
    0.923856; 0.382672; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.498550; 0.498550; -0.709098;,
    0.651418; 0.269814; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    0.269814; 0.651418; -0.709098;,
    0.498550; 0.498550; -0.709098;,
    0.498550; 0.498550; -0.709098;,
    0.269814; 0.651418; -0.709098;,
    0.382672; 0.923856; 0.000000;,
    0.707083; 0.707083; 0.000000;,
    0.707083; 0.707083; 0.000000;,
    0.382672; 0.923856; 0.000000;,
    0.269814; 0.651418; 0.709098;,
    0.498550; 0.498550; 0.709098;,
    0.498550; 0.498550; 0.709098;,
    0.269814; 0.651418; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.269814; 0.651418; 0.709098;,
    0.000000; 0.705069; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.382672; 0.923856; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 0.705069; 0.709098;,
    0.269814; 0.651418; 0.709098;,
    0.269814; 0.651418; -0.709098;,
    0.000000; 0.705069; -0.709098;,
    0.000000; 1.000000; 0.000000;,
    0.382672; 0.923856; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.705069; -0.709098;,
    0.269814; 0.651418; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    -0.269814; 0.651418; -0.709098;,
    0.000000; 0.705069; -0.709098;,
    0.000000; 0.705069; -0.709098;,
    -0.269814; 0.651418; -0.709098;,
    -0.382672; 0.923856; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    -0.382672; 0.923856; 0.000000;,
    -0.269814; 0.651418; 0.709098;,
    0.000000; 0.705069; 0.709098;,
    0.000000; 0.705069; 0.709098;,
    -0.269814; 0.651418; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.269814; 0.651418; 0.709098;,
    -0.498550; 0.498550; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.382672; 0.923856; 0.000000;,
    -0.707083; 0.707083; 0.000000;,
    -0.498550; 0.498550; 0.709098;,
    -0.269814; 0.651418; 0.709098;,
    -0.269814; 0.651418; -0.709098;,
    -0.498550; 0.498550; -0.709098;,
    -0.707083; 0.707083; 0.000000;,
    -0.382672; 0.923856; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    -0.498550; 0.498550; -0.709098;,
    -0.269814; 0.651418; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    -0.651418; 0.269814; -0.709098;,
    -0.498550; 0.498550; -0.709098;,
    -0.498550; 0.498550; -0.709098;,
    -0.651418; 0.269814; -0.709098;,
    -0.923856; 0.382672; 0.000000;,
    -0.707083; 0.707083; 0.000000;,
    -0.707083; 0.707083; 0.000000;,
    -0.923856; 0.382672; 0.000000;,
    -0.651418; 0.269814; 0.709098;,
    -0.498550; 0.498550; 0.709098;,
    -0.498550; 0.498550; 0.709098;,
    -0.651418; 0.269814; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.651418; 0.269814; 0.709098;,
    -0.705069; 0.000000; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.923856; 0.382672; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -0.705069; 0.000000; 0.709098;,
    -0.651418; 0.269814; 0.709098;,
    -0.651418; 0.269814; -0.709098;,
    -0.705069; 0.000000; -0.709098;,
    -1.000000; 0.000000; 0.000000;,
    -0.923856; 0.382672; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    -0.705069; 0.000000; -0.709098;,
    -0.651418; 0.269814; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    -0.651418; -0.269814; -0.709098;,
    -0.705069; 0.000000; -0.709098;,
    -0.705069; 0.000000; -0.709098;,
    -0.651418; -0.269814; -0.709098;,
    -0.923856; -0.382672; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -1.000000; 0.000000; 0.000000;,
    -0.923856; -0.382672; 0.000000;,
    -0.651418; -0.269814; 0.709098;,
    -0.705069; 0.000000; 0.709098;,
    -0.705069; 0.000000; 0.709098;,
    -0.651418; -0.269814; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.651418; -0.269814; 0.709098;,
    -0.498550; -0.498550; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.923856; -0.382672; 0.000000;,
    -0.707083; -0.707083; 0.000000;,
    -0.498550; -0.498550; 0.709098;,
    -0.651418; -0.269814; 0.709098;,
    -0.651418; -0.269814; -0.709098;,
    -0.498550; -0.498550; -0.709098;,
    -0.707083; -0.707083; 0.000000;,
    -0.923856; -0.382672; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    -0.498550; -0.498550; -0.709098;,
    -0.651418; -0.269814; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    -0.269814; -0.651418; -0.709098;,
    -0.498550; -0.498550; -0.709098;,
    -0.498550; -0.498550; -0.709098;,
    -0.269814; -0.651418; -0.709098;,
    -0.382672; -0.923856; 0.000000;,
    -0.707083; -0.707083; 0.000000;,
    -0.707083; -0.707083; 0.000000;,
    -0.382672; -0.923856; 0.000000;,
    -0.269814; -0.651418; 0.709098;,
    -0.498550; -0.498550; 0.709098;,
    -0.498550; -0.498550; 0.709098;,
    -0.269814; -0.651418; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.269814; -0.651418; 0.709098;,
    0.000000; -0.705069; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    -0.382672; -0.923856; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -0.705069; 0.709098;,
    -0.269814; -0.651418; 0.709098;,
    -0.269814; -0.651418; -0.709098;,
    0.000000; -0.705069; -0.709098;,
    0.000000; -1.000000; 0.000000;,
    -0.382672; -0.923856; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.000000; -0.705069; -0.709098;,
    -0.269814; -0.651418; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    0.269814; -0.651418; -0.709098;,
    0.000000; -0.705069; -0.709098;,
    0.000000; -0.705069; -0.709098;,
    0.269814; -0.651418; -0.709098;,
    0.382672; -0.923856; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.382672; -0.923856; 0.000000;,
    0.269814; -0.651418; 0.709098;,
    0.000000; -0.705069; 0.709098;,
    0.000000; -0.705069; 0.709098;,
    0.269814; -0.651418; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.269814; -0.651418; 0.709098;,
    0.498550; -0.498550; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.382672; -0.923856; 0.000000;,
    0.707083; -0.707083; 0.000000;,
    0.498550; -0.498550; 0.709098;,
    0.269814; -0.651418; 0.709098;,
    0.269814; -0.651418; -0.709098;,
    0.498550; -0.498550; -0.709098;,
    0.707083; -0.707083; 0.000000;,
    0.382672; -0.923856; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.498550; -0.498550; -0.709098;,
    0.269814; -0.651418; -0.709098;,
    0.000000; 0.000000; -1.000000;,
    0.651418; -0.269814; -0.709098;,
    0.498550; -0.498550; -0.709098;,
    0.498550; -0.498550; -0.709098;,
    0.651418; -0.269814; -0.709098;,
    0.923856; -0.382672; 0.000000;,
    0.707083; -0.707083; 0.000000;,
    0.707083; -0.707083; 0.000000;,
    0.923856; -0.382672; 0.000000;,
    0.651418; -0.269814; 0.709098;,
    0.498550; -0.498550; 0.709098;,
    0.498550; -0.498550; 0.709098;,
    0.651418; -0.269814; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.651418; -0.269814; 0.709098;,
    0.705069; 0.000000; 0.709098;,
    0.000000; 0.000000; 1.000000;,
    0.923856; -0.382672; 0.000000;,
    1.000000; 0.000000; 0.000000;,
    0.705069; 0.000000; 0.709098;,
    0.651418; -0.269814; 0.709098;,
    0.651418; -0.269814; -0.709098;,
    0.705069; 0.000000; -0.709098;,
    1.000000; 0.000000; 0.000000;,
    0.923856; -0.382672; 0.000000;,
    0.000000; 0.000000; -1.000000;,
    0.705069; 0.000000; -0.709098;,
    0.651418; -0.269814; -0.709098;;
64;
3; 0, 1, 2;,
4; 3, 4, 5, 6;,
4; 7, 8, 9, 10;,
3; 11, 12, 13;,
3; 14, 15, 16;,
4; 17, 18, 19, 20;,
4; 21, 22, 23, 24;,
3; 25, 26, 27;,
3; 28, 29, 30;,
4; 31, 32, 33, 34;,
4; 35, 36, 37, 38;,
3; 39, 40, 41;,
3; 42, 43, 44;,
4; 45, 46, 47, 48;,
4; 49, 50, 51, 52;,
3; 53, 54, 55;,
3; 56, 57, 58;,
4; 59, 60, 61, 62;,
4; 63, 64, 65, 66;,
3; 67, 68, 69;,
3; 70, 71, 72;,
4; 73, 74, 75, 76;,
4; 77, 78, 79, 80;,
3; 81, 82, 83;,
3; 84, 85, 86;,
4; 87, 88, 89, 90;,
4; 91, 92, 93, 94;,
3; 95, 96, 97;,
3; 98, 99, 100;,
4; 101, 102, 103, 104;,
4; 105, 106, 107, 108;,
3; 109, 110, 111;,
3; 112, 113, 114;,
4; 115, 116, 117, 118;,
4; 119, 120, 121, 122;,
3; 123, 124, 125;,
3; 126, 127, 128;,
4; 129, 130, 131, 132;,
4; 133, 134, 135, 136;,
3; 137, 138, 139;,
3; 140, 141, 142;,
4; 143, 144, 145, 146;,
4; 147, 148, 149, 150;,
3; 151, 152, 153;,
3; 154, 155, 156;,
4; 157, 158, 159, 160;,
4; 161, 162, 163, 164;,
3; 165, 166, 167;,
3; 168, 169, 170;,
4; 171, 172, 173, 174;,
4; 175, 176, 177, 178;,
3; 179, 180, 181;,
3; 182, 183, 184;,
4; 185, 186, 187, 188;,
4; 189, 190, 191, 192;,
3; 193, 194, 195;,
3; 196, 197, 198;,
4; 199, 200, 201, 202;,
4; 203, 204, 205, 206;,
3; 207, 208, 209;,
3; 210, 211, 212;,
4; 213, 214, 215, 216;,
4; 217, 218, 219, 220;,
3; 221, 222, 223;;
}  //End of MeshNormals
  }  // End of the Mesh Sphere 
  }  // SI End of the Object Sphere 
}  // End of the Root Frame
