using System.Collections.Generic;

namespace Nez.AI.UtilityAI
{
	/// <summary>
	///     Scores by summing the score of all child Appraisals
	/// </summary>
	public class SumOfChildrenConsideration<T> : IConsideration<T>
    {
        private readonly List<IAppraisal<T>> _appraisals = new List<IAppraisal<T>>();
        public IAction<T> action { get; set; }


        float IConsideration<T>.getScore(T context)
        {
            var score = 0f;
            for (var i = 0; i < _appraisals.Count; i++)
                score += _appraisals[i].getScore(context);

            return score;
        }
    }
}