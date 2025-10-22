using FAI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;


namespace FAI.Common
{
    // statische Hilfsklasse für allgemeine Hilfsmethoden
    // Statisch bedeutet, dass die Klasse nicht instanziert werden kann und alle ihre Mitglieder ebenfalls statisch sein müssen.
    public static class Helpers
    {
        // Reflection-Methode zum Abbilden von Eigenschaften zwischen zwei Objekten

        public static void MapEntityProperties<TSource, TTarget>(TSource source, TTarget target, List<string> excludeProperties)
            where TSource : class
            where TTarget : class, IEntity
        {
            // Typen der Quell- und Zielobjekte abrufen
            var sourceType = source.GetType();
            var targetType = target.GetType();

            // Sicherstellen, dass Quell- und Zieltypen von der gleichen Basisklasse erben
            if (sourceType.BaseType.FullName != targetType.BaseType.FullName)
            {
                throw new ApplicationException("Source and target must be of the same type.");
            }

            // Alle öffentlichen Instanz-Eigenschaften der Quell- und Zieltypen abrufen
            var targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            targetProperties.ForEach(p =>
            {
                
                // Prüft ob die Eigenschaft beschreibbar ist (CanWrite)
                // Ist der Name der Eigenschaft nicht in der Ausschlussliste excludeProperties enthalten?
                // Falls excludeProperties null ist, wird stattdessen eine leere Liste verwendet([]).
                // Die Bedingung ist nur wahr, wenn die Eigenschaft einen Setter hat und nicht ausgeschlossen werden soll.
                if (p.CanWrite && !(excludeProperties ?? []).Contains(p.Name))
                {
                    // Passende Eigenschaft im Quellobjekt anhand des Namens suchen
                    var sourceProperty = sourceType.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                    
                    if (sourceProperty != null)
                    {
                        // Wert der Quell-Eigenschaft abrufen
                        var sourcePropertyValue = sourceProperty.GetValue(source, null);
                        // Wert der Quell-Eigenschaft in die Ziel-Eigenschaft schreiben
                        p.SetValue(target, sourcePropertyValue, null);
                    }
                }
            });

        }
            
    }
}
