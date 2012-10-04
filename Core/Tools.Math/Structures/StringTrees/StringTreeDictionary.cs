﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Math.Structures.StringTrees
{
    /// <summary>
    /// A hyper efficient data structure to search and index values by string identifiers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>Inpiration/main algoritmic idea from Fast Algorithms for Sorting and Searching Strings, Jon L. Bentley and Robert Sedgewick</remarks>
    public class StringTreeDictionary<T>
    {
        /// <summary>
        /// The root of the dictionary.
        /// </summary>
        private StringTreeNode _root;

        /// <summary>
        /// Creates a new empty dictionary.
        /// </summary>
        public StringTreeDictionary()
        {
            _root = new StringTreeNode((char)(short.MaxValue / 2));
        }

        /// <summary>
        /// Adds a value indexed by a given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, T value)
        {
            _root.Add(0, key, value);
        }

        /// <summary>
        /// Returns the value for the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T SearchExact(string key)
        {
            if (_root != null)
            {
                return _root.SearchExact(0, key);
            }
            return default(T);
        }

        /// <summary>
        /// Represents a single node in the search tree.
        /// </summary>
        private class StringTreeNode
        {
            /// <summary>
            /// The lower node.
            /// </summary>
            private StringTreeNode _lower_node;

            /// <summary>
            /// The equal node.
            /// </summary>
            private StringTreeNode _equal_node;

            /// <summary>
            /// The higher node.
            /// </summary>
            private StringTreeNode _higher_node;

            /// <summary>
            /// The split character.
            /// </summary>
            private char _split_char;

            /// <summary>
            /// The value of this node.
            /// </summary>
            private T _value;

            /// <summary>
            /// Creates a new string tree node.
            /// </summary>
            /// <param name="split_char"></param>
            public StringTreeNode(char split_char)
            {
                // set the split char.
                _split_char = split_char;
            }

            /// <summary>
            /// Adds a new value at this node.
            /// </summary>
            /// <param name="idx"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            internal void Add(short idx, string key, T value)
            {
                // TODO: improve on this by inserting the mean value first.
                // TODO: improve on this by remove recursion and going to iteration.
                char s = key[idx];
                if (s < _split_char)
                { // s belongs at the low side.
                    // recursively add to the low side.
                    if (_lower_node == null)
                    { // there is no lower node; create one.
                        _lower_node = new StringTreeNode(key[idx]);
                        _lower_node.Add((short)(idx + 1), key, value);
                    }
                    else
                    { // add to the lower node.
                        _lower_node.Add(idx, key, value);
                    }
                }
                else if (s == _split_char)
                { // s belongs at the equal node.
                    if (idx < key.Length - 1)
                    { // there are still other chars left!
                        if (_equal_node == null)
                        { // there is no equal node; create one.
                            _equal_node = new StringTreeNode(key[idx]);
                        }
                        // add to the equals node.
                        _equal_node.Add((short)(idx + 1), key, value);
                    }
                    else
                    { // there are no other char's left! assign the value here!
                        _value = value; // no recursion anymore, it ends here!
                    }
                }
                else
                { // s belongs at the high side.
                    // recursively add to the high side.
                    if (_higher_node == null)
                    { // there is no higher node; create one.
                        _higher_node = new StringTreeNode(key[idx]);
                        _higher_node.Add((short)(idx + 1), key, value);
                    }
                    else
                    { // add to the lower node.
                        _higher_node.Add(idx, key, value);
                    }
                }
            }

            /// <summary>
            /// Searches for the value for the given key.
            /// </summary>
            /// <param name="idx"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            internal T SearchExact(short idx, string key)
            {
                // TODO: improve on this by remove recursion and going to iteration.
                char s = key[idx];
                if (s < _split_char)
                { // s belongs at the low side.
                    if (_lower_node != null)
                    { // search the lower node.
                        return _lower_node.SearchExact(idx, key);
                    }
                }
                else if (s == _split_char)
                { // s belongs at the equal node.
                    if (idx < key.Length - 1)
                    { // there are still other chars left!
                        if (_equal_node != null)
                        { // there is no equal node; create one.
                            return _equal_node.SearchExact(idx, key);
                        }
                    }
                    else
                    { // there are no other char's left! the value is here!
                        return _value; // no recursion anymore, it ends here!
                    }
                }
                else
                { // s belongs at the high side.
                    if (_higher_node != null)
                    { // search the high node.
                        return _higher_node.SearchExact(idx, key);
                    }
                }
                return default(T);
            }
        }
    }
}