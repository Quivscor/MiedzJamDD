using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ExpeditionMap
{
    public class Expedition
    {
        public Field destinationField = null;

        private int timeToFinishExpedition = 0;
        private int currentTimeToFinishExpedition = 0;

        public Expedition(Field field)
        {
            this.destinationField = field;

            timeToFinishExpedition = field.DistanceToRoot + 2;

            TimeController.Instance.OnNextDay += OnNextDay;
        }

        private void OnNextDay()
        {
            currentTimeToFinishExpedition++;

            if (currentTimeToFinishExpedition >= timeToFinishExpedition)
            {
                ExpeditionFinished();
            }
        }

        private void ExpeditionFinished()
        {
            TimeController.Instance.OnNextDay -= OnNextDay;
            ExpeditionManager.Instance.ExpeditionFinished(OnExpeditionFinished());
        }

        public RaportData OnExpeditionFinished()
        {
            destinationField?.SetAsExpeditionTarget(false);

            CalculateGainingCopper();

            CalculateDiscoveringNeighbourFields();

            CalculateChancesForAdditionalActions();

            return GenerateRaport();
        }

        private RaportData GenerateRaport()
        {
            RaportData raportData = new RaportData();

            raportData.fieldCoordinatesText = "[ " + destinationField.FieldCoords.x + ", " + destinationField.FieldCoords.y + " ]";

            raportData.gainedCopperText = gainedCopper + "";

            raportData.gainedCopper = gainedCopper;

            raportData.discoveredFieldsText = discoveredFields + "";

            raportData.contextText = GenerateRaportContent(); //Change to static method for generating context or something

            raportData.additionalActivity = additionalActivityOccured;

            raportData.additionalActivityText = additionalActivityOutput;

            return raportData;
        }

        private int gainedCopper = 0;
        private int discoveredFields = 0;
        private bool additionalActivityOccured = false;
        private string additionalActivityOutput = "";

        public int CurrentTimeToFinishExpedition { get => currentTimeToFinishExpedition;  }
        public int LeftTimeToFinishExpedition { get => timeToFinishExpedition - currentTimeToFinishExpedition; }

        private void CalculateGainingCopper()
        {
            int maxGainedCopper = (int)Mathf.Min((int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Transport].level * TeamStatsModifiers.LoadModifier, destinationField.CurrentCopper);

            destinationField.CurrentCopper = (int)Mathf.Max(destinationField.CurrentCopper - maxGainedCopper, 0);

            gainedCopper = maxGainedCopper;

            //ResourceController.Instance?.AddCopper(gainedCopper);
        }

        private void CalculateDiscoveringNeighbourFields()
        {
            Field[] neighbours = ExpeditionMapManager.Instance.GetAllNeighbours(destinationField);

            System.Random random = new System.Random();

            neighbours = neighbours.OrderBy(x => random.Next()).ToArray();

            int fieldsToDiscover = 1;

            if (Random.Range(0, 100) <= TeamStats.Instance.ChanceToDiscoverAdditionalNeighbourField)
                fieldsToDiscover++;

            discoveredFields = 0;

            for (int i = 0; i < neighbours.Length; i++)
            {
                if (!neighbours[i].HasBeenDiscovered)
                {
                    neighbours[i].HasBeenDiscovered = true;
                    fieldsToDiscover--;
                    discoveredFields++;

                    if (fieldsToDiscover <= 0)
                        break;
                }
            }
        }

        private void CalculateChancesForAdditionalActions()
        {
            //Implement later
            //Add smth to managers or increase smth

            if (Random.Range(0, 100) < (int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Telecommunication].level * TeamStatsModifiers.CommunicationModifier)
            {
                additionalActivityOccured = true;

                //80% points
                if (Random.Range(0, 100) < 80)
                {
                    int pointsToAdd = Random.Range(1, 4);

                    int category = Random.Range(0, 4);

                    int textRandom = Random.Range(0, 2);

                    switch (category)
                    {
                        case 0:
                            switch (textRandom)
                            {
                                case 0:
                                    //Dzięki wysoko rozwiniętej strukturze telekomunikacyjnej w trakcie ekspedycji dodatkowo udało się zmierzyć wydajność najnowszych części zamontowanych w naszych maszynach.
                                    additionalActivityOutput += "Thanks to our highly developed telecommunication network, during the expedition we managed to measure performance of our newest parts in our new machines.\n";
                                    break;
                                default:
                                    //Wysoko rozwinięta struktura telekomunikacyjna pozwoliła na zbadanie i zoptymalizowanie działań górników.
                                    additionalActivityOutput += "Highly developed telecommunication network allowed us to examine and enhance the work of our miners.\n";
                                    break;
                            }
                            //Rozwój kategorii <color=#FFFA34>" + (CategoriesProgressController.ScienceCategory)category + "</color> został zwiększony o <color=#FFFA34>" + pointsToAdd + "</color> " + (pointsToAdd > 1 ? "punkty." : "punkt.")
                            additionalActivityOutput += "Development in <color=#FFFA34>" + (CategoriesProgressController.ScienceCategory)category + "</color> category was increased by <color=#FFFA34>" + pointsToAdd + "</color> " + (pointsToAdd > 1 ? "points." : "point.");
                            break;
                        case 1:
                            switch (textRandom)
                            {
                                case 0:
                                    //Zaawansowana struktura telekomunikacyjna pozwoliła dodatkowo usprawnić działanie radarów w trakcie ekspedycji.
                                    additionalActivityOutput += "Advanced telecommunication network allowed us to improve radar efficiency during expeditions.\n";
                                    break;
                                default:
                                    //W trakcie ekspedycji zarejestrowane zostały bliżej niezidentyfikowane sygnały.
                                    additionalActivityOutput += "During expedtion we registered unidentified signals.\n";
                                    break;
                            }
                            additionalActivityOutput += "Development in <color=#B900F8>" + (CategoriesProgressController.ScienceCategory)category + "</color> category was increased by <color=#B900F8>" + pointsToAdd + "</color> " + (pointsToAdd > 1 ? "points." : "point.");
                            break;
                        case 2:
                            switch (textRandom)
                            {
                                case 0:
                                    //Zaawansowana struktura telekomunikacyjna pozwoliła prześledzić i zoptymalizować trasę naszych maszyn.
                                    additionalActivityOutput += "Advanced telecommunication network allowed us to track and improve routes of our machines.\n";
                                    break;
                                default:
                                    //Nasze radary wykryły i wyznaczyły nowe podziemne trasy dla naszych maszyn.
                                    additionalActivityOutput += "\n";
                                    break;
                            }
                            additionalActivityOutput += "Rozwój kategorii <color=#CD1725>" + (CategoriesProgressController.ScienceCategory)category + "</color> został zwiększony o <color=#CD1725>" + pointsToAdd + "</color> " + (pointsToAdd > 1 ? "punkty." : "punkt.");
                            break;
                        case 3:
                            switch (textRandom)
                            {
                                case 0:
                                    additionalActivityOutput += "Nasze radary wykryły nową, nieznaną do tej pory substancję w trakcie ekspedycji. Technicy-górnicy zabezpieczyli ją i pracują obecnie nad wykorzystaniem jej jako nawozu do naszych upraw.\n";
                                    break;
                                default:
                                    additionalActivityOutput += "Wysoko rozwinięta struktura telekomunikacyjna pozwoliła na sprawniejszą komunikację między zespołami i lepszą organizację zadań.\n";
                                    break;
                            }
                            additionalActivityOutput += "Rozwój kategorii <color=#00B917>" + (CategoriesProgressController.ScienceCategory)category + "</color> został zwiększony o <color=#00B917>" + pointsToAdd + "</color> " + (pointsToAdd > 1 ? "punkty." : "punkt.");
                            break;
                    }

                    CategoriesProgressController.Instance.AddPointsToScience((CategoriesProgressController.ScienceCategory)category, pointsToAdd);
                }
                //20% copper
                else
                {
                    int copperToAdd = Random.Range(1, 4) * 50;

                    int textRandom = Random.Range(0, 2);

                    switch (textRandom)
                    {
                        case 0:
                            additionalActivityOutput += "Nasze radary wykryły w trakcie ekspedycji nowe złoża miedzi. Organizowana jest obecnie ekspedycja mająca przeprowadzić szczegółowe badania na ten temat.\n";
                            break;
                        default:
                            additionalActivityOutput += "Dzięki wysoko rozwiniętej strukturze teleinformatycznej prace przebiegły szybciej niż początkowo zakładano.\n";
                            break;
                    }
                    additionalActivityOutput += "Otrzymano dodatkowo <color=yellow>" + copperToAdd + " miedzi</color>.";

                    ResourceController.Instance.AddCopper(copperToAdd);
                }
            }
            else
            {
                additionalActivityOccured = false;
                additionalActivityOutput = "Nothing happened";
            }
        }

        public string GenerateRaportContent()
        {
            int lengthFluffOption = Random.Range(0, 5);
            string expeditionLength = "";
            string miesiecy = "miesiące";
            if (timeToFinishExpedition >= 5)
                miesiecy = "miesięcy";
            switch (lengthFluffOption)
            {
                case 0:
                    expeditionLength = "Ekspedycja potrwała " + timeToFinishExpedition + " " + miesiecy +".\n";
                    break;
                case 1:
                    expeditionLength = "Prace kopalniane trzymały górników w pracy przez " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;
                case 2:
                    expeditionLength = "Po " + timeToFinishExpedition + " miesiącach prace zostały zakończone.\n";
                    break;
                case 3:
                    expeditionLength = "Długość wyprawy wyniosła " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;
                case 4:
                    expeditionLength = "Drążenie tuneli i pozyskanie miedzi zajęło nam " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;

            }

            string miningStory = "";
            lengthFluffOption = Random.Range(0, 5);
            switch (lengthFluffOption)
            {
                case 0:
                    miningStory = "Prace przebiegły normalnie. Nie odnotowano żadnych wypadków ani zniszczeń.\n";
                    break;
                case 1:
                    miningStory = "Zanotowano wykorzystanie materiałów wybuchowych podczas wydobycia.\n";
                    break;
                case 2:
                    miningStory = "Część drążonego tunelu składała się z bardzo twardego materiału, ale nasze maszyny dały sobie radę.\n";
                    break;
                case 3:
                    miningStory = "Wydobyte pokłady miedzi pokryły się z naszymi szacowaniami.\n";
                    break;
                case 4:
                    miningStory = "Mimo rekordowych upałów udało się sprawnie przeprowadzić ekspedycję.\n";
                    break;
            }

            return expeditionLength + miningStory;
        }
    }
}
