using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.AOC2023.Day5;
internal class CategoryEvaluator
{
	public Dictionary<Category, CategoryMapper> destinations = [];

	public CategoryEvaluator(CategoryMapper[] mappers)
	{
		destinations = mappers.ToDictionary(m => m.Destination);
	}

	public long Evaluate(Category source, long sourceValue, Category destination)
	{
		var mappers = new List<CategoryMapper>();
		var curMapper = destinations[destination];
		do
		{
			mappers.Add(curMapper);
			curMapper = destinations[curMapper.Source];
		} while (curMapper.Source != source);
		mappers.Add(destinations[mappers.Last().Source]);
		mappers.Reverse();
		var result = sourceValue;
		foreach (var mapper in mappers)
		{
			result = mapper.Evaluate(result);
		}
		return result;
	}

	public (long start, long end)[] Evaluate(Category source, (long start, long end)[] seeds, Category destination)
	{
		var mappers = new List<CategoryMapper>();
		var curMapper = destinations[destination];
		do
		{
			mappers.Add(curMapper);
			curMapper = destinations[curMapper.Source];
		} while (curMapper.Source != source);
		mappers.Add(destinations[mappers.Last().Source]);
		mappers.Reverse();
		var result = seeds;
		foreach (var mapper in mappers)
		{
			result = mapper.Evaluate(result);
		}
		return result.Distinct().ToArray();
	}

	public CategoryMapper GetCategoryMapper(Category destination)
	{
		return destinations[destination];
	}


	public enum Category
	{
		Seed,
		Soil,
		Fertilizer,
		Water,
		Light,
		Temperature,
		Humidity,
		Location
	}
}
