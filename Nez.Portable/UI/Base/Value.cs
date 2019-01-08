namespace Nez.UI
{
	/// <summary>
	///     Value placeholder, allowing the value to be computed on request. Values are provided an element for context which
	///     reduces the
	///     number of value instances that need to be created and reduces verbosity in code that specifies values
	/// </summary>
	public abstract class Value
    {
	    /// <summary>
	    ///     A value that is always zero.
	    /// </summary>
	    public static Fixed zero = new Fixed(0);


        public static Value minWidth = new MinWidthValue();


        public static Value minHeight = new MinHeightValue();


        public static Value prefWidth = new PrefWidthValue();


        public static Value prefHeight = new PrefHeightValue();


        public static Value maxWidth = new MaxWidthValue();


        public static Value maxHeight = new MaxHeightValue();

        /// <summary>
        ///     context May be null
        /// </summary>
        /// <param name="context">Context.</param>
        public abstract float get(Element context);


        /// <summary>
        ///     Value that is the maxHeight of the element in the cell.
        /// </summary>
        public static Value percentWidth(float percent)
        {
            return new PercentWidthValue
            {
                percent = percent
            };
        }


        /// <summary>
        ///     Returns a value that is a percentage of the specified elements's width. The context element is ignored.
        /// </summary>
        public static Value percentWidth(float percent, Element delegateElement)
        {
            return new PercentWidthDelegateValue
            {
                delegateElement = delegateElement,
                percent = percent
            };
        }


        /// <summary>
        ///     Returns a value that is a percentage of the element's height.
        /// </summary>
        public static Value percentHeight(float percent)
        {
            return new PercentageHeightValue
            {
                percent = percent
            };
        }


        /// <summary>
        ///     Returns a value that is a percentage of the specified elements's height. The context element is ignored.
        /// </summary>
        public static Value percentHeight(float percent, Element delegateElement)
        {
            return new PercentHeightDelegateValue
            {
                delegateElement = delegateElement,
                percent = percent
            };
        }


        /// <summary>
        ///     A fixed value that is not computed each time it is used.
        /// </summary>
        public class Fixed : Value
        {
            private readonly float value;

            public Fixed(float value)
            {
                this.value = value;
            }

            public override float get(Element context)
            {
                return value;
            }
        }

        /// <summary>
        ///     Value that is the minWidth of the element in the cell.
        /// </summary>
        public class MinWidthValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).minWidth;
                return context == null ? 0 : context.width;
            }
        }

        /// <summary>
        ///     Value that is the minHeight of the element in the cell.
        /// </summary>
        public class MinHeightValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).minHeight;
                return context == null ? 0 : context.height;
            }
        }

        /// <summary>
        ///     Value that is the prefWidth of the element in the cell.
        /// </summary>
        public class PrefWidthValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).preferredWidth;
                return context == null ? 0 : context.width;
            }
        }

        /// <summary>
        ///     Value that is the prefHeight of the element in the cell.
        /// </summary>
        public class PrefHeightValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).preferredHeight;
                return context == null ? 0 : context.height;
            }
        }

        /// <summary>
        ///     Value that is the maxWidth of the element in the cell.
        /// </summary>
        public class MaxWidthValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).maxWidth;
                return context == null ? 0 : context.width;
            }
        }

        /// <summary>
        ///     Value that is the maxHeight of the element in the cell.
        /// </summary>
        public class MaxHeightValue : Value
        {
            public override float get(Element context)
            {
                if (context is ILayout)
                    return ((ILayout) context).maxHeight;
                return context == null ? 0 : context.height;
            }
        }

        /// <summary>
        ///     Returns a value that is a percentage of the element's width.
        /// </summary>
        public class PercentWidthValue : Value
        {
            public float percent;

            public override float get(Element element)
            {
                return element.width * percent;
            }
        }

        /// <summary>
        ///     Returns a value that is a percentage of the specified elements's width. The context element is ignored.
        /// </summary>
        public class PercentWidthDelegateValue : Value
        {
            public Element delegateElement;
            public float percent;

            public override float get(Element element)
            {
                return delegateElement.width * percent;
            }
        }

        /// <summary>
        ///     Returns a value that is a percentage of the element's height.
        /// </summary>
        public class PercentageHeightValue : Value
        {
            public float percent;

            public override float get(Element element)
            {
                return element.height * percent;
            }
        }

        /// <summary>
        ///     Returns a value that is a percentage of the specified elements's height. The context element is ignored.
        /// </summary>
        public class PercentHeightDelegateValue : Value
        {
            public Element delegateElement;
            public float percent;

            public override float get(Element element)
            {
                return delegateElement.height * percent;
            }
        }
    }
}