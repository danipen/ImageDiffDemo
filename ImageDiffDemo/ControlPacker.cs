using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace ImageDiffDemo
{
    internal static class ControlPacker
    {
        internal static void AddControls(NSView parent, string[] constraints, NSDictionary controls)
        {
            foreach (NSView control in controls.Values)
            {
                control.TranslatesAutoresizingMaskIntoConstraints = false;

                parent.AddSubview(control);
            }

            parent.AddConstraints(BuildConstraints(constraints, controls));
        }

        internal static void Fill(NSView parent, NSView child)
        {
            ControlPacker.AddControls(
                parent,
                new string[]{
                    "H:|[child]|",
                    "V:|[child]|"
                },
                new NSDictionary("child", child));
        }

        static NSLayoutConstraint[] BuildConstraints(string[] constraints, NSDictionary views)
        {
            List<NSLayoutConstraint> result = new List<NSLayoutConstraint>();

            foreach (string format in constraints)
            {
                result.AddRange(NSLayoutConstraint.FromVisualFormat(format, 0, null, views));
            }

            return result.ToArray();
        }
    }
}