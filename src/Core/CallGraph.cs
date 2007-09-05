/* 
 * Copyright (C) 1999-2007 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */

using Decompiler.Core;
using Decompiler.Core.Lib;
using System;
using System.Collections;
using System.IO;

namespace Decompiler.Core
{
	/// <summary>
	/// Describes the structure of the graph: what nodes call what others.
	/// </summary>
	public class CallGraph
	{
		private Procedure [] postOrderProcs;
		private Procedure [] rpoProcs;
		private ArrayList entryPoints = new ArrayList();	
		private Hashtable procedures = new Hashtable();
		private DirectedGraph graphProcs = new DirectedGraph();
		private DirectedGraph graphStms = new DirectedGraph();
		private int invdfs;

		public void AddEdge(Statement stmCaller, Procedure callee)
		{
			graphProcs.AddNode(stmCaller.block.Procedure);
			graphProcs.AddNode(callee);
			graphProcs.AddEdge(stmCaller.block.Procedure, callee);

			graphStms.AddNode(stmCaller);
			graphStms.AddNode(callee);
			graphStms.AddEdge(stmCaller, callee);
		}

		public void AddEntryPoint(Procedure proc)
		{
			AddProcedure(proc);
			if (!entryPoints.Contains(proc))
			{
				entryPoints.Add(proc);
			}
		}

		public void AddProcedure(Procedure proc)
		{
			graphProcs.AddNode(proc);
			graphStms.AddNode(proc);
		}

		public void AddStatement(Statement stm)
		{
			graphStms.AddNode(stm);
		}

		public ICollection Callees(Statement stm)
		{
			return graphStms.Successors(stm);
		}

		public ICollection Callees(Procedure proc)
		{
			return graphProcs.Successors(proc);
		}

		public ICollection CallerProcedures(Procedure proc)
		{
			return graphProcs.Predecessors(proc);
		}

		public ICollection CallerStatements(Procedure proc)
		{
			return graphStms.Predecessors(proc);
		}

		public void Emit(TextWriter wri)
		{
			SortedList sl = new SortedList(new ProcedureComparer());
			foreach (Procedure proc in graphProcs.Nodes)
			{
				sl.Add(proc, proc);
			}
			foreach (Procedure proc in sl.Values)
			{
				wri.WriteLine("Procedure {0} calls:", proc.Name);
				foreach (Procedure p in graphProcs.Successors(proc))
				{
					wri.WriteLine("\t{0}", p.Name);
				}
			}
		}

		private class ProcedureComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				Procedure a = (Procedure) x;
				Procedure b = (Procedure) y;
				return string.Compare(a.Name, b.Name);
			}

			#endregion

		}


		public ArrayList EntryPoints
		{
			get { return entryPoints; }
		}

		public ICollection PostOrderProcedures
		{
			get { return postOrderProcs; }
		}

		private void Renumber(Procedure n, Hashtable visited)
		{
			if (!visited.Contains(n))
			{
				visited[n] = n;
				ArrayList callees = new ArrayList();
				foreach (Procedure proc in graphProcs.Successors(n))
				{
					callees.Add(proc);
				}
				int c = callees.Count;
				for (int i = c - 1; i >= 0; --i)
				{
					Procedure callee = (Procedure) callees[i];
					Renumber(callee, visited);
				}
				
				n.PostOrderNumber = invdfs;
				postOrderProcs[invdfs] = n;
				++invdfs;
				n.RpoNumber = graphProcs.Nodes.Count - invdfs;
				rpoProcs[n.RpoNumber] = n;
			}
		}

		public void RenumberProcedures()
		{
			rpoProcs = new Procedure[graphProcs.Nodes.Count];
			postOrderProcs = new Procedure[graphProcs.Nodes.Count];

			Hashtable visited = new Hashtable(graphProcs.Nodes.Count);
			invdfs = 0;
			foreach (Procedure p in entryPoints)
			{
				Renumber(p, visited);
			}

			foreach (Procedure p in graphProcs.Nodes)
			{
				Renumber(p, visited);
			}
		}
	}
}
