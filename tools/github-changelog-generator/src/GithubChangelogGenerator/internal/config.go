package internal

type Config struct {
	Repo      string   `mapstructure:"repo"`
	Token     string   `mapstructure:"token"`
	Milestone string   `mapstructure:"milestone"`
	SinceTag  string   `mapstructure:"since-tag"`
	UntilTag  string   `mapstructure:"until-tag"`
	State     string   `mapstructure:"state"`
	Groups    []*Group `mapstructure:"groups"`
	Template  string   `mapstructure:"template"`
}

type Group struct {
	Labels []string `mapstructure:"labels"`
	Title  string   `mapstructure:"title"`
}

func AllLabels(groups []*Group) map[int][]string {
	allLabels := make(map[int][]string)
	for i, group := range groups {
		if allLabels[i] == nil {
			allLabels[i] = make([]string, 0)
		}
		allLabels[i] = append(allLabels[i], group.Labels...)
	}
	return allLabels
}
