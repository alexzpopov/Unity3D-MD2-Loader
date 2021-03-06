﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Surface  {

	public List<Vector3> vert_coords ;
	public List<Vector3> vert_norm;
	public List<Vector2> vert_tex_coords0;
	public List<Vector2> vert_tex_coords1;
	public List<int> tris;
	public string name;
	private bool UseUV2;
	private bool UseNormals;
	private Mesh mesh;
	private int no_verts;
	private int no_tris;



	public  Surface (string name) 
	{
		vert_coords= new List<Vector3>();
		vert_norm= new List<Vector3>();
		vert_tex_coords0= new List<Vector2>();
		vert_tex_coords1= new List<Vector2>();
		tris = new  List<int>();
		this.name = name;
		this.UseNormals=false;
		this.UseUV2=false;
		mesh = new Mesh();
		mesh.name = name;
		no_verts = 0;
		no_tris = 0;
	}

	public void Optimize()
	{
		mesh.Optimize();
	}
	public void RecalculateNormals()
	{
		mesh.RecalculateNormals ();
	}

	public void build()
	{
		mesh.vertices = vert_coords.ToArray();
		mesh.uv = vert_tex_coords0.ToArray();
		mesh.triangles = tris.ToArray();
		if (UseUV2) 
		{
			mesh.uv2=vert_tex_coords1.ToArray();
		}
		
		if (UseNormals) 
		{
			mesh.normals=vert_norm.ToArray();
		} else 
		{
			mesh.RecalculateNormals();
		}
		mesh.RecalculateBounds ();
	}
	public Mesh getMesh()
	{
		return mesh;
    }

	
	public void VertexNormal(int vid ,float nx,float ny,float nz)
	{
		vert_norm[vid] = new Vector3 (nx, ny, nz);
	}
	public void VertexCoords(int vid ,float x,float y,float z)
	{
		vert_coords [vid] = new Vector3 (x, y, z);

	}
	public void VertexTexCoords( int vid,float u,float v,float w,int coords_set)
	{
		if(coords_set==0)
		{
			vert_tex_coords0[vid]=new Vector2(u,v);
		}else{
			vert_tex_coords1[vid]=new Vector2(u,v);
			UseUV2=true;			
		}
	}
	public int AddVertex(float x,float y, float z)
	{
		no_verts++;
		vert_coords.Add(new Vector3(x,y,z));
		vert_norm.Add (Vector3.zero);
		vert_tex_coords0.Add (Vector2.zero);
		vert_tex_coords1.Add (Vector2.zero);
		return no_verts-1;
		
	}
	public int AddVertex(float x,float y, float z,float u,float v)
	{
		no_verts++;
		vert_coords.Add(new Vector3(x,y,z));
		vert_tex_coords0.Add (new Vector2 (u, v));
		return no_verts-1;
		
	}
	public int AddVertex(float x,float y, float z,float nx,float ny,float nz,float u,float v)
	{
		no_verts++;
		vert_coords.Add(new Vector3(x,y,z));
		vert_norm.Add(new Vector3(nx,ny,nz));
		vert_tex_coords0.Add (new Vector2 (u, v));
		UseNormals = true;
		return no_verts-1;
		
	}
	public int AddVertex(float x,float y, float z,float u0,float v0,float u1,float v1)
	{
		no_verts++;
		vert_coords.Add(new Vector3(x,y,z));
		vert_tex_coords0.Add (new Vector2 (u0, v0));
		vert_tex_coords1.Add (new Vector2 (u1, v1));
		UseUV2 = true;
		return no_verts-1;
		
	}
	public int AddVertex(float x,float y, float z,float nx,float ny,float nz,float u0,float v0,float u1,float v1)
	{
		no_verts++;
		vert_coords.Add(new Vector3(x,y,z));

		vert_norm.Add(new Vector3(nx,ny,nz));
		vert_tex_coords0.Add (new Vector2 (u0, v0));
		vert_tex_coords1.Add (new Vector2 (u1, v1));
		UseUV2 = true;
		return no_verts-1;
		
	}
	public int  AddTriangle(int v0,int v1,int v2)
	{
		
		no_tris++;
		tris.Add(v0);
		tris.Add(v1);
		tris.Add(v2);
		return no_tris;
	}
	public int addFace(Vector3 v0,Vector3 v1,Vector3 v2,Vector2 uv0,Vector2 uv1,Vector2 uv2)
	{
		
		int f0=this.AddVertex(v0.x, v0.y, v0.z, uv0.x, uv0.y);
		int f1=this.AddVertex(v1.x, v1.y, v1.z, uv1.x, uv1.y);
		int f2=this.AddVertex(v2.x, v2.y, v2.z, uv2.x, uv2.y);
		return AddTriangle(f0, f1, f2);
		
		
	}
	public int addFace(Vector3 v0,Vector3 v1,Vector3 v2,Vector3 n0,Vector3 n1,Vector3 n2,Vector2 uv0,Vector2 uv1,Vector2 uv2)
	{
		
		int f0=this.AddVertex(v0.x, v0.y, v0.z,n0.x,n0.y,n0.z, uv0.x, uv0.y);
		int f1=this.AddVertex(v1.x, v1.y, v1.z,n1.x,n1.y,n1.z, uv1.x, uv1.y);
		int f2=this.AddVertex(v2.x, v2.y, v2.z,n2.x,n2.y,n2.z, uv2.x, uv2.y);

		return AddTriangle(f0, f1, f2);
		
		
	}
	public  int CountFaces()
	{
		return (tris.Count/3);
	}
	public  int CountTriangles()
	{
		return no_tris;
	} 
	public  int CountVertices()
	{
		return no_verts;
	}
	public  int getIndice(int index)
	{
		return tris[index];
	}
	public  void setIndice(int index,int indice)
	{
		 tris[index]=indice;
	}

	public  int getFace(int numface,int index)
	{
		return tris[numface * 3 + index];
	}
	public  Vector3 getVertexFace(int numface,int index)
	{
		return getVertex(tris[numface * 3 + index]);
	}
	public  void setVertexFace(int numface,int index,Vector3 v)
	{
		 setVertex(tris[numface * 3 + index],v);
	}
	public Vector3 getVertex(int index)
	{
		return vert_coords[index];
	}
	public void setVertex(int index,Vector3 v)
	{
		vert_coords [index] = v;
	}

	public static Mesh createCube(string name)
	{
		Surface surf = new Surface (name);

		surf.AddVertex(-1.0f,-1.0f,-1.0f);
		surf.AddVertex(-1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f,-1.0f,-1.0f);
		
		surf.AddVertex(-1.0f,-1.0f, 1.0f);
		surf.AddVertex(-1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f,-1.0f, 1.0f);
		
		surf.AddVertex(-1.0f,-1.0f, 1.0f);
		surf.AddVertex(-1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f,-1.0f, 1.0f);
		
		surf.AddVertex(-1.0f,-1.0f,-1.0f);
		surf.AddVertex(-1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f,-1.0f,-1.0f);
		
		surf.AddVertex(-1.0f,-1.0f, 1.0f);
		surf.AddVertex(-1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f, 1.0f, 1.0f);
		surf.AddVertex( 1.0f,-1.0f, 1.0f);
		
		surf.AddVertex(-1.0f,-1.0f,-1.0f);
		surf.AddVertex(-1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f, 1.0f,-1.0f);
		surf.AddVertex( 1.0f,-1.0f,-1.0f);
		
		surf.VertexNormal(0,0.0f,0.0f,-1.0f);
		surf.VertexNormal(1,0.0f,0.0f,-1.0f);
		surf.VertexNormal(2,0.0f,0.0f,-1.0f);
		surf.VertexNormal(3,0.0f,0.0f,-1.0f);
		
		surf.VertexNormal(4,0.0f,0.0f,1.0f);
		surf.VertexNormal(5,0.0f,0.0f,1.0f);
		surf.VertexNormal(6,0.0f,0.0f,1.0f);
		surf.VertexNormal(7,0.0f,0.0f,1.0f);
		
		surf.VertexNormal(8,0.0f,-1.0f,0.0f);
		surf.VertexNormal(9,0.0f,1.0f,0.0f);
		surf.VertexNormal(10,0.0f,1.0f,0.0f);
		surf.VertexNormal(11,0.0f,-1.0f,0.0f);
		
		surf.VertexNormal(12,0.0f,-1.0f,0.0f);
		surf.VertexNormal(13,0.0f,1.0f,0.0f);
		surf.VertexNormal(14,0.0f,1.0f,0.0f);
		surf.VertexNormal(15,0.0f,-1.0f,0.0f);
		
		surf.VertexNormal(16,-1.0f,0.0f,0.0f);
		surf.VertexNormal(17,-1.0f,0.0f,0.0f);
		surf.VertexNormal(18,1.0f,0.0f,0.0f);
		surf.VertexNormal(19,1.0f,0.0f,0.0f);
		
		surf.VertexNormal(20,-1.0f,0.0f,0.0f);
		surf.VertexNormal(21,-1.0f,0.0f,0.0f);
		surf.VertexNormal(22,1.0f,0.0f,0.0f);
		surf.VertexNormal(23,1.0f,0.0f,0.0f);
		
		surf.VertexTexCoords(0,0.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(1,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(2,1.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(3,1.0f,1.0f,0.0f,0);
		
		surf.VertexTexCoords(4,1.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(5,1.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(6,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(7,0.0f,1.0f,0.0f,0);
		
		surf.VertexTexCoords(8,0.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(9,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(10,1.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(11,1.0f,1.0f,0.0f,0);
		
		surf.VertexTexCoords(12,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(13,0.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(14,1.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(15,1.0f,0.0f,0.0f,0);
		
		surf.VertexTexCoords(16,0.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(17,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(18,1.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(19,1.0f,1.0f,0.0f,0);
		
		surf.VertexTexCoords(20,1.0f,1.0f,0.0f,0);
		surf.VertexTexCoords(21,1.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(22,0.0f,0.0f,0.0f,0);
		surf.VertexTexCoords(23,0.0f,1.0f,0.0f,0);
		
		surf.VertexTexCoords(0,0.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(1,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(2,1.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(3,1.0f,1.0f,0.0f,1);
		
		surf.VertexTexCoords(4,1.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(5,1.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(6,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(7,0.0f,1.0f,0.0f,1);
		
		surf.VertexTexCoords(8,0.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(9,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(10,1.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(11,1.0f,1.0f,0.0f,1);
		
		surf.VertexTexCoords(12,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(13,0.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(14,1.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(15,1.0f,0.0f,0.0f,1);
		
		surf.VertexTexCoords(16,0.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(17,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(18,1.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(19,1.0f,1.0f,0.0f,1);
		
		surf.VertexTexCoords(20,1.0f,1.0f,0.0f,1);
		surf.VertexTexCoords(21,1.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(22,0.0f,0.0f,0.0f,1);
		surf.VertexTexCoords(23,0.0f,1.0f,0.0f,1);
		
		surf.AddTriangle(0,1,2); // front
		surf.AddTriangle(0,2,3);
		surf.AddTriangle(6,5,4); // back
		surf.AddTriangle(7,6,4);
		surf.AddTriangle(6+8,5+8,1+8); // top
		surf.AddTriangle(2+8,6+8,1+8);
		surf.AddTriangle(0+8,4+8,7+8); // bottom
		surf.AddTriangle(0+8,7+8,3+8);
		surf.AddTriangle(6+16,2+16,3+16); // right
		surf.AddTriangle(7+16,6+16,3+16);
		surf.AddTriangle(0+16,1+16,5+16); // left
		surf.AddTriangle(0 + 16, 5 + 16, 4 + 16);
		surf.build ();
		return surf.getMesh ();

	}
}
