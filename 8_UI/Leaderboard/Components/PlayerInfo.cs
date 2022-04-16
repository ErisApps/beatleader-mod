using BeatLeader.Manager;
using BeatLeader.Models;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace BeatLeader.Components 
{
    [ViewDefinition(Plugin.ResourcesPath + ".BSML.Components.PlayerInfo.bsml")]
    internal class PlayerInfo : ReeUIComponent 
    {
        #region Components

        [UIValue("avatar"), UsedImplicitly]
        private PlayerAvatar _avatar = Instantiate<PlayerAvatar>(false);

        #endregion

        #region Initialize/Dispose

        protected override void OnInitialize() {
            LeaderboardEvents.UserProfileStartedEvent += OnProfileRequestStarted;
            LeaderboardEvents.UserProfileFetchedEvent += OnProfileFetched;
        }

        protected override void OnDispose() {
            LeaderboardEvents.UserProfileStartedEvent -= OnProfileRequestStarted;
            LeaderboardEvents.UserProfileFetchedEvent -= OnProfileFetched;
        }

        #endregion

        #region Events

        private void OnProfileRequestStarted() {
            NameText = "Loading...";
            StatsActive = false;
        }

        private void OnProfileFetched(Player player) {
            if (player == null) {
                NameText = "Error";
                StatsActive = false;
            } else {
                _countryFlag.SetCountry(player.country);
                _avatar.SetAvatar(player.avatar);
                NameText = FormatUtils.FormatUserName(player.name);
                GlobalRankText = FormatUtils.FormatRank(player.rank, true);
                CountryRankText = FormatUtils.FormatRank(player.countryRank, true);
                PpText = FormatUtils.FormatPP(player.pp);
                StatsActive = true;
            }
        }

        #endregion

        #region CountryRankImage

        [UIValue("country-flag"), UsedImplicitly]
        private CountryFlag _countryFlag = Instantiate<CountryFlag>();

        #endregion

        #region StatsActive

        private bool _statsActive;

        [UIValue("stats-active"), UsedImplicitly]
        public bool StatsActive {
            get => _statsActive;
            set {
                if (_statsActive.Equals(value)) return;
                _statsActive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region NameText

        private string _nameText = "";

        [UIValue("name-text"), UsedImplicitly]
        public string NameText {
            get => _nameText;
            set {
                if (_nameText.Equals(value)) return;
                _nameText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region GlobalRankText

        private string _globalRankText = "";

        [UIValue("global-rank-text"), UsedImplicitly]
        public string GlobalRankText {
            get => _globalRankText;
            set {
                if (_globalRankText.Equals(value)) return;
                _globalRankText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region CountryRankText

        private string _countryRankText = "";

        [UIValue("country-rank-text"), UsedImplicitly]
        public string CountryRankText {
            get => _countryRankText;
            set {
                if (_countryRankText.Equals(value)) return;
                _countryRankText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region PpText

        private string _ppText = "";

        [UIValue("pp-text"), UsedImplicitly]
        public string PpText {
            get => _ppText;
            set {
                if (_ppText.Equals(value)) return;
                _ppText = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}