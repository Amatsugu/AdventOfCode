using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rule = (int after, int before);


namespace AdventOfCode.Problems.AOC2024.Day5;
internal class PageList
{
	private readonly List<Rule> _rules;
	private PageNode _root;

	public PageList(int[] pages, List<Rule> rules)
	{
		_rules = rules.Where(r => pages.Contains(r.before)).ToList();
		_root = new PageNode(pages[0], GetRulesForPage(pages[0]));
		for (int i = 1; i < pages.Length; i++)
			AddPage(pages[i]);
	}

	public void AddPage(int page)
	{
		var node = new PageNode(page, GetRulesForPage(page));
		if(node.IsBefore(_root))
		{
			node.NextNode = _root;
			_root = node;
			return;
		}
		var curNode = _root;
		while(curNode != null)
		{
			if(curNode.IsBefore(node))
			{
				if(curNode.NextNode == null)
				{
					curNode.NextNode = node;
					break;
				}
				else if(node.IsBefore(curNode.NextNode))
				{
					node.NextNode = curNode.NextNode;
					curNode.NextNode = node;
					break;
				}

			}
			curNode = curNode.NextNode;
		}
	}

	public List<Rule> GetRulesForPage(int page)
	{
		return _rules.Where(r => r.after == page).ToList();
	}

	public List<int> Traverse()
	{
		var list = new List<int>();
		var curNode = _root;
		while(curNode != null)
		{
			list.Add(curNode.Page);
			curNode = curNode.NextNode;
		}
		return list;
	}
}


internal class PageNode
{
	public int Page { get; set; }
	public int[] Before { get; set; }
	public PageNode? NextNode { get; set; }

	public PageNode(int page, List<Rule> rules)
	{
		Page = page;
		Before = rules.Count == 0 ? [int.MinValue] : rules.Select(r => r.before).ToArray();
	}

	public override string ToString()
	{
		return $"{Page} << {Before}";
	}

	public bool IsBefore(PageNode other)
	{
		if(Before.Any(b => b == other.Page))
			return true;
		return false;
	}
}