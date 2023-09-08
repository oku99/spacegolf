
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-bob-fans";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "bob fans";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Fernando Agustin Teodoro", "fteodoro@student.unimelb.edu.au"),
        new TeamMember("Gaku Sakashita", "gsakashita@student.unimelb.edu.au"),
        new TeamMember("Hao En See", "haoens@student.unimelb.edu.au"),
        // Remove the following line if you have a group of 3
        new TeamMember("James Amanatidis", "jamanatidis@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "Space Golf";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
@"Procedurally-generated minigolf game, where you play on a galactic scale. The objective of the game 
is to hit the golf ball into the hole within a designated number of turns to successfully move on to the next map. 
Our minigolf game has a catch, which is existence of planet entities that causes the golf ball to curve, due to being 
affected by their respective gravities and wormholes around it.
";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://www.youtube.com/watch?v=_1U9YG00irQ";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
