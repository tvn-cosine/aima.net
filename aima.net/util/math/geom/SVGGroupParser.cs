 

namespace aima.net.util.math.geom
{


    ////    /**
    ////    * This class : {@link IGroupParser} for a SVG map.
    ////    * the "g" element is used to define the group(s) that should be parsed.<br/>
    ////    * 
    ////    * The parser only recognizes the following basic shapes:
    ////    * <ul>
    ////    * <li>rect</li>
    ////    * <li>line</li>
    ////    * <li>circle</li>
    ////    * <li>ellipse</li>
    ////    * <li>polyline</li>
    ////    * <li>polygon</li>
    ////    * </ul>
    ////    * In addition any number of grouping elements are allowed.<br/>
    ////    * For all elements only the coordinates and the transform attribute are used. This means that rounded corners etc. are ignored.
    ////    * Every element/shape can use the transform attribute. The following transformations may be used:<br/><br/>
    ////    * <ul>
    ////    * <li>translate</li>
    ////    * <li>scale</li>
    ////    * <li>rotate</li>
    ////    * </ul> 
    ////    * To use the svg map with {@code CartesianPlot2D} it has to contain a "g" element with an id.<br/>
    ////    * See <a href="https://www.w3.org/TR/SVG/expanded-toc.html">w3c&reg; SVG standard definition</a> for more information.<br/>
    ////    * <br/>
    ////    * During the process of parsing most of the time is spent in the {@link XMLStreamReader}.
    ////    * A known issue is a <code><!DOCTYPE ></code> element in the file. Removing this element can speed up the parsing significantly.
    ////    * 
    ////    * @author Arno von Borries
    ////    * @author Jan Phillip Kretzschmar
    ////    * @author Andreas Walscheid
    ////    *
    ////    */
    ////    public class SVGGroupParser : IGroupParser
    ////    {
    ////        private const string GROUP_ELEMENT = "g";
    ////        private const string CIRCLE_ELEMENT = "circle";
    ////        private const string ELLIPSE_ELEMENT = "ellipse";
    ////        private const string LINE_ELEMENT = "line";
    ////        private const string POLYLINE_ELEMENT = "polyline";
    ////        private const string POLYGON_ELEMENT = "polygon";
    ////        private const string RECT_ELEMENT = "rect";
    ////        private const string ID_ATTRIBUTE = "id";
    ////        private const string TRANSFORM_ATTRIBUTE = "transform";
    ////        private const string X_ATTRIBUTE = "x";
    ////        private const string Y_ATTRIBUTE = "y";
    ////        private const string CX_ATTRIBUTE = "cx";
    ////        private const string CY_ATTRIBUTE = "cy";
    ////        private const string X1_ATTRIBUTE = "x1";
    ////        private const string Y1_ATTRIBUTE = "y1";
    ////        private const string X2_ATTRIBUTE = "x2";
    ////        private const string Y2_ATTRIBUTE = "y2";
    ////        private const string R_ATTRIBUTE = "r";
    ////        private const string RX_ATTRIBUTE = "rx";
    ////        private const string RY_ATTRIBUTE = "ry";
    ////        private const string WIDTH_ATTRIBUTE = "width";
    ////        private const string HEIGHT_ATTRIBUTE = "height";
    ////        private const string POINTS_ATTRIBUTE = "points";
    ////        private const string TRANSLATE_TRANSFORM = "translate";
    ////        private const string SCALE_TRANSFORM = "scale";
    ////        private const string ROTATE_TRANSFORM = "rotate";
    ////        private const string POINTS_REGEX = "[,\\s]+";
    ////        private const string TRANSFORM_REGEX1 = "[a-zA-Z]*\\([0-9.,Ee\\+\\-\\s]*\\)";
    ////        private const string TRANSFORM_REGEX2 = "([a-zA-Z]+)|([0-9\\.Ee\\+\\-]*[eEmMxXpPiInNcCtT%]*[^\\,\\(\\)\\s]+)";
    ////        private const string NUMBER_REGEX = "([\\+\\-]?[0-9]+\\.?[0-9]*[Ee]?[\\+\\-]?[0-9]*\\.?[0-9]*)|em|ex|px|in|cm|mm|pt|pc|\\%";

    ////        private static readonly IRegularExpressionNUMBER_PATTERN = TextFactory.CreateRegularExpression(NUMBER_REGEX);
    ////        private static readonly IRegularExpressionTRANSFORM_PATTERN1 = TextFactory.CreateRegularExpression(TRANSFORM_REGEX1);
    ////        private static readonly IRegularExpressionTRANSFORM_PATTERN2 = TextFactory.CreateRegularExpression(TRANSFORM_REGEX2);

    ////        private static readonly XMLInputFactory FACTORY = XMLInputFactory.newInstance();

    ////	    private XMLStreamReader reader;
    ////        private ArrayList<IGeometric2D> shapes;
    ////        private Stack<TransformMatrix2D> transformations = new Stack<TransformMatrix2D>();
    ////        private TransformMatrix2D currentMatrix;

    ////        /**
    ////         * Parses the given {@link InputStream} into a group of geometric shapes.
    ////         * @param input the given input stream.
    ////         * @param groupID the identifier for the group.
    ////         * @throws XMLStreamException if a syntax error is found in the input.
    ////         * @return the constructed list of geometric shapes.
    ////         */

    ////        public ArrayList<IGeometric2D> parse(InputStream input, string groupID) throws XMLStreamException
    ////        {
    ////		if(input == null || groupID == null) throw new NullPointerException();
    ////        reader = FACTORY.createXMLStreamReader(input);
    ////		shapes = Factory.CreateQueue<IGeometric2D>();
    ////		transformations.Clear();
    ////		currentMatrix = TransformMatrix2D.UNITY_MATRIX;
    ////		while(reader.hasNext()) {
    ////		      final int event = reader.next();
    ////		      if (event == XMLStreamConstants.START_ELEMENT) {
    ////            applyTransform();
    ////            if (reader.getLocalName().equalsIgnoreCase(GROUP_ELEMENT))
    ////            {
    ////                final string element = reader.getAttributeValue(null, ID_ATTRIBUTE);
    ////                if (element != null)
    ////                {
    ////                    if (element.equalsIgnoreCase(groupID))
    ////                    {
    ////                        parseGroup();
    ////                        break;
    ////                    }
    ////                }
    ////            }
    ////        } else if(event == XMLStreamConstants.END_ELEMENT) {
    ////            applyTransformEnd();
    ////        }
    ////        }
    ////		return shapes;
    ////	}

    ////    /**
    ////     * Parses the specified group.
    ////     * @throws XMLStreamException if an syntax error was encountered in the file.
    ////     */
    ////    private void parseGroup()  
    ////    {
    ////		int groupCounter = 1;
    ////		while (reader.hasNext()) {
    ////            int event = reader.next();
    ////            if (event == XMLStreamConstants.START_ELEMENT) {
    ////                applyTransform();
    ////                final string elementName = reader.getLocalName();
    ////                if (elementName.equalsIgnoreCase(CIRCLE_ELEMENT)) parseCircle();
    ////                else if (elementName.equalsIgnoreCase(ELLIPSE_ELEMENT)) parseEllipse();
    ////                else if (elementName.equalsIgnoreCase(LINE_ELEMENT)) parseLine();
    ////                else if (elementName.equalsIgnoreCase(POLYLINE_ELEMENT)) parsePolyline();
    ////                else if (elementName.equalsIgnoreCase(POLYGON_ELEMENT)) parsePolygon();
    ////                else if (elementName.equalsIgnoreCase(RECT_ELEMENT)) parseRect();
    ////            } else if (event == XMLStreamConstants.END_ELEMENT) {
    ////                applyTransformEnd();
    ////                if (reader.getLocalName().equalsIgnoreCase(GROUP_ELEMENT))
    ////                {
    ////                    groupCounter--;
    ////                    if (groupCounter == 0) break;
    ////                }
    ////            }
    ////        }
    ////    }

    ////    /**
    ////     * Checks the current element for a transform attribute and adds that attribute to the current transform matrix.
    ////     * Manages the transformation stack.
    ////     */
    ////    private void applyTransform()
    ////    {
    ////        string value = reader.getAttributeValue(null, TRANSFORM_ATTRIBUTE);
    ////        transformations.push(currentMatrix);
    ////        currentMatrix = currentMatrix.multiply(parseTransform(value));
    ////    }

    ////    /**
    ////     * Sets the current transform matrix to the matrix of the underlying element when leaving an element.
    ////     * Manages the transformation stack.
    ////     */
    ////    private void applyTransformEnd()
    ////    {
    ////        currentMatrix = transformations.pop();
    ////    }

    ////    /**
    ////     * This method parses a transform attribute into a {@link TransformMatrix2D}.<br/>
    ////     * @param string the string of the transform attribute.
    ////     * @return the parsed transform matrix.
    ////     */
    ////    private TransformMatrix2D parseTransform(string string)
    ////    {
    ////        TransformMatrix2D result = TransformMatrix2D.UNITY_MATRIX;
    ////        if (string != null)
    ////        {
    ////            Matcher matcher1 = TRANSFORM_PATTERN1.matcher(string);
    ////            int transformCount1 = 0;
    ////            while (matcher1.lookingAt()) transformCount1++;
    ////            for (int j = 1; j <= transformCount1; j++)
    ////            {
    ////                Matcher matcher2 = TRANSFORM_PATTERN2.matcher(matcher1.group(j));
    ////                int transformCount2 = 0;
    ////                while (matcher1.lookingAt()) transformCount2++;
    ////                for (int i = 1; i < transformCount2;)
    ////                {
    ////                    if (matcher2.group(i).equalsIgnoreCase(TRANSLATE_TRANSFORM))
    ////                    {
    ////                        double tx = parseNumber(matcher2.group(++i));
    ////                        double ty = 0.0d;
    ////                       ++i;
    ////                        try
    ////                        {
    ////                            ty = parseNumber(matcher2.group(i));
    ////                           ++i;
    ////                        }
    ////                        catch (NumberFormatException e)
    ////                        {
    ////                            e.printStackTrace();
    ////                        }
    ////                        result = result.multiply(TransformMatrix2D.translate(tx, ty));
    ////                    }
    ////                    else if (matcher2.group(i).equalsIgnoreCase(SCALE_TRANSFORM))
    ////                    {
    ////                        double sx = parseNumber(matcher2.group(++i));
    ////                        double sy = sx;
    ////                       ++i;
    ////                        try
    ////                        {
    ////                            sy = parseNumber(matcher2.group(i));
    ////                           ++i;
    ////                        }
    ////                        catch (NumberFormatException e)
    ////                        {
    ////                            e.printStackTrace();
    ////                        }
    ////                        result = result.multiply(TransformMatrix2D.scale(sx, sy));
    ////                    }
    ////                    else if (matcher2.group(i).equalsIgnoreCase(ROTATE_TRANSFORM))
    ////                    {
    ////                        double angle = Math.toRadians(parseNumber(matcher2.group(++i)));
    ////                        double cx = 0.0d;
    ////                        double cy = 0.0d;
    ////                       ++i;
    ////                        try
    ////                        {
    ////                            cx = parseNumber(matcher2.group(i));
    ////                           ++i;
    ////                            cy = parseNumber(matcher2.group(i));
    ////                           ++i;
    ////                        }
    ////                        catch (NumberFormatException e)
    ////                        {
    ////                            e.printStackTrace();
    ////                        }
    ////                        if (cx != 0 && cy != 0)
    ////                        {
    ////                            result = result.multiply(TransformMatrix2D.translate(cx, cy));
    ////                        }
    ////                        result = result.multiply(TransformMatrix2D.rotate(angle));
    ////                        if (cx != 0 && cy != 0)
    ////                        {
    ////                            result = result.multiply(TransformMatrix2D.translate(-cx, -cy));
    ////                        }
    ////                    }
    ////                    else
    ////                    {
    ////                       ++i;
    ////                    }
    ////                }
    ////            }
    ////        }
    ////        return result;
    ////    }

    ////    /**
    ////     * Parses the current element as a rectangle. This rectangle is added to the {@code shapes} if it is rendered.
    ////     */
    ////    private void parseRect()
    ////    {
    ////        string value = reader.getAttributeValue(null, X_ATTRIBUTE);
    ////        final double x = parseNumber(value);
    ////        value = reader.getAttributeValue(null, Y_ATTRIBUTE);
    ////        final double y = parseNumber(value);
    ////        value = reader.getAttributeValue(null, WIDTH_ATTRIBUTE);
    ////        final double width = parseNumber(value);
    ////        value = reader.getAttributeValue(null, HEIGHT_ATTRIBUTE);
    ////        final double height = parseNumber(value);
    ////        if (width != 0.0d && height != 0.0d)
    ////        {
    ////            //SVG standard specifies that both width and height are forced to have a value. Otherwise the rendering for this element is disabled.
    ////            IGeometric2D rect = new Rect2D(x, y, x + width, y + height).transform(currentMatrix);
    ////            shapes.Add(rect);
    ////        }
    ////    }

    ////    /**
    ////     * Parses the current element as a circle. This circle is added to the {@code shapes} if it is rendered.
    ////     */
    ////    private void parseCircle()
    ////    {
    ////        string value = reader.getAttributeValue(null, CX_ATTRIBUTE);
    ////        final double cx = parseNumber(value);
    ////        value = reader.getAttributeValue(null, CY_ATTRIBUTE);
    ////        final double cy = parseNumber(value);
    ////        value = reader.getAttributeValue(null, R_ATTRIBUTE);
    ////        final double r = parseNumber(value);
    ////        if (r != 0.0d)
    ////        {
    ////            //SVG standard specifies that the radius is forced to have a value. Otherwise the rendering for this element is disabled.
    ////            IGeometric2D circle = new Circle2D(new Point2D(cx, cy), r).transform(currentMatrix);
    ////            shapes.Add(circle);
    ////        }
    ////    }

    ////    /**
    ////     * Parses the current element as an ellipse. This ellipse is added to the {@code shapes} if it is rendered.
    ////     */
    ////    private void parseEllipse()
    ////    {
    ////        string value = reader.getAttributeValue(null, CX_ATTRIBUTE);
    ////        final double cx = parseNumber(value);
    ////        value = reader.getAttributeValue(null, CY_ATTRIBUTE);
    ////        final double cy = parseNumber(value);
    ////        value = reader.getAttributeValue(null, RX_ATTRIBUTE);
    ////        final double rx = parseNumber(value);
    ////        value = reader.getAttributeValue(null, RY_ATTRIBUTE);
    ////        final double ry = parseNumber(value);
    ////        if (rx != 0.0d && ry != 0.0d)
    ////        {
    ////            //SVG standard specifies that the radius is forced to have a value. Otherwise the rendering for this element is disabled.
    ////            IGeometric2D elipse = new Ellipse2D(new Point2D(cx, cy), rx, ry).transform(currentMatrix);
    ////            shapes.Add(elipse);
    ////        }
    ////    }

    ////    /**
    ////     * Parses the current element as a line.
    ////     */
    ////    private void parseLine()
    ////    {
    ////        string value = reader.getAttributeValue(null, X1_ATTRIBUTE);
    ////        final double x1 = parseNumber(value);
    ////        value = reader.getAttributeValue(null, Y1_ATTRIBUTE);
    ////        final double y1 = parseNumber(value);
    ////        value = reader.getAttributeValue(null, X2_ATTRIBUTE);
    ////        final double x2 = parseNumber(value);
    ////        value = reader.getAttributeValue(null, Y2_ATTRIBUTE);
    ////        final double y2 = parseNumber(value);
    ////        IGeometric2D line = new Line2D(x1, y1, x2, y2).transform(currentMatrix);
    ////        shapes.Add(line);
    ////    }

    ////    /**
    ////     * Parses the current element as a polyline. This polyline is added to the {@code shapes} if it contains a valid points list.
    ////     */
    ////    private void parsePolyline()
    ////    {
    ////        string value = reader.getAttributeValue(null, POINTS_ATTRIBUTE);
    ////        if (value != null)
    ////        {
    ////            String[] coords = value.split(POINTS_REGEX);
    ////            if (coords.Length >= 2 && coords.Length % 2 == 0)
    ////            {
    ////                //otherwise something is wrong with the points list!
    ////                Point2D[] vertexes = new Point2D[coords.Length / 2];
    ////                for (int i = 0; i < coords.Length; i = i + 2)
    ////                {
    ////                    vertexes[(i / 2) - 1] = new Point2D(parseNumber(coords[i]), parseNumber(coords[i + 1]));
    ////                }
    ////                IGeometric2D polyline = new Polyline2D(vertexes, false).transform(currentMatrix);
    ////                shapes.Add(polyline);
    ////            }
    ////        }
    ////    }

    ////    /**
    ////     * Parses the current element as a polygon. This polygon is added to the {@code shapes} if it contains a valid points list.
    ////     */
    ////    private void parsePolygon()
    ////    {
    ////        string value = reader.getAttributeValue(null, POINTS_ATTRIBUTE);
    ////        if (value != null)
    ////        {
    ////            String[] coords = value.split(POINTS_REGEX);
    ////            if (coords.Length >= 2 && coords.Length % 2 == 0)
    ////            {
    ////                //otherwise something is wrong with the points list!
    ////                Point2D[] vertexes = new Point2D[coords.Length / 2];
    ////                for (int i = 1; i < coords.Length; i = i + 2)
    ////                {
    ////                    vertexes[(i - 1) / 2] = new Point2D(parseNumber(coords[i - 1]), parseNumber(coords[i]));
    ////                }
    ////                IGeometric2D polygon = new Polyline2D(vertexes, true).transform(currentMatrix);
    ////                shapes.Add(polygon);
    ////            }
    ////        }
    ////    }

    ////    /**
    ////     * Parses a given string as a number. The valid format of the number is specified in the SVG standard. It is parsed through the regular expression {@code NUMBER_PATTERN}.
    ////     * @param string the string containing the number.
    ////     * @return the number as a double.
    ////     */
    ////    private double parseNumber(string string)
    ////    {
    ////        if (string == null) return 0.0d;
    ////        Matcher matcher = NUMBER_PATTERN.matcher(string);
    ////        if (!matcher.lookingAt()) return 0.0d;
    ////        final string group = matcher.group(1);
    ////        if (group == null) return 0.0d;
    ////        return double.valueOf(group);
    ////    }
    ////}

}
