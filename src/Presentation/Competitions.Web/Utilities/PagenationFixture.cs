namespace Competitions.Web.Utilities
{
	public class PagenationFixture
	{
		public static int Calc(int skip, int take, int total)
		{
			int i = 0;
			if (skip > 0)
				i = skip - take;
			if (skip > take)
				i = skip - (2 * take);
			if (skip + (4 * take) > total && total > (4 * take) && skip > take)
				i = skip - (2 * take);
			if (skip + (3 * take) > total && total > (3 * take) && skip > take * 2)
				i = skip - (2 * take);
			if (skip + (2 * take) > total && total > (2 * take) && skip > take * 3)
				i = skip - (3 * take);
			if (skip + take > total && total > take && skip > take * 4)
				i = skip - (4 * take);

			return i;
		}
	}
}
