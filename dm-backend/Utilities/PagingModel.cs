using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dm_backend.Models;
public class PagedList<T> : List<T>
{
	public int CurrentPage { get; private set; }
	public int TotalPages { get; private set; }
	public int PageSize { get; private set; }
	public int TotalCount { get; private set; }

	public bool HasPrevious => CurrentPage > 1;
	public bool HasNext => CurrentPage < TotalPages;

	public PagedList(List<T> items,int count, int pageNumber, int pageSize)
	{
		TotalCount = count;
		PageSize = pageSize;
		CurrentPage = pageNumber;
		TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

		AddRange(items);
	}

	public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
	{	pageNumber= pageNumber<1 ? 1 : pageNumber;
		pageSize= pageSize<1 ? 5 : pageSize;
		var count = source.Count();
		var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

		return new PagedList<T>(items,count,pageNumber, pageSize);
	}
	public object getMetaData()
            {
               return new { TotalCount=TotalCount,
                PageSize=PageSize,
                CurrentPage=CurrentPage,
                TotalPages=TotalPages,
                HasNext=HasNext,
                HasPrevious=HasPrevious
			   };
            }
}