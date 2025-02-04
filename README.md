# TrainingPeaks Console App

This console application can be used to lookup the following data for a single user, or list of users:

**Reps** - The cumulative amount of reps a user has completed for an exercise.

**Weight** - The cumulative amount of weight a user has lifted for an exercise.

**Total Weight** - The cumulative amount of total weight (**reps** * **weight**) a user has lifted for an exercise. 

**Personal Record** - The highest amount of **reps**, **weight**, or **total weight** a user has achieved for a given exercise.

## Running The App

#### Building project with `dotnet`



If opting to build the project from source, the following sequence of commands will build a release version and navigate to the app:

```dos
> dotnet build --configuration Release 
> cd bin\Release\net8.0
> .\trainingpeaks --help
```

#### Downloading the release `.zip` file.

Alternatively, the release version can be downloaded in [releases section of this repo](https://github.com/acampos81/trainingpeaks/releases/tag/release-1.2).  Once the `.zip` file is extracted, navigate to the `trainingpeaks` directory to run commands.

## Test Questions JSON Output

The following commands and flags were used to answer the test questions provided.  Console command instructions are provided in the **How To** section further below.

> How many total pounds have all of these athletes combined Bench Pressed?

**Answer**: 5,548,625 lbs

```dos
> .\trainingpeaks workout -rw --users 5101,9705,15321,22677,29891 --exercise 568
```

```json
{
  "exercise": {
    "id": 568,
    "title": "Bench Press"
  },
  "users": [
    {
      "id": 5101,
      "name_first": "Abby",
      "name_last": "Smith",
      "total_weight": 1110350
    },
    {
      "id": 9705,
      "name_first": "Dave",
      "name_last": "Jones",
      "total_weight": 975035
    },
    {
      "id": 15321,
      "name_first": "Becky",
      "name_last": "Davis",
      "total_weight": 1230900
    },
    {
      "id": 22677,
      "name_first": "Barry",
      "name_last": "Moore",
      "total_weight": 1088145
    },
    {
      "id": 29891,
      "name_first": "Jamie",
      "name_last": "Johnston",
      "total_weight": 1144195
    }
  ],
  "combined_total_weight": 5548625
}
```

#
> How many total pounds did Barry Moore Back Squat in 2016?

**Answer**: 512,935 lbs

```dos
.\trainingpeaks workout -rw --users 22677 --exercise 326 --dates 2016-01-01 2016-12-31
```

```json
{
  "exercise": {
    "id": 326,
    "title": "Back Squat"
  },
  "users": [
    {
      "id": 22677,
      "name_first": "Barry",
      "name_last": "Moore",
      "total_weight": 512935
    }
  ],
  "combined_total_weight": 512935
}
```

#
> In what month of 2017 did Barry Moore Back Squat the most total weight?

**Answer**: December

```dos
.\trainingpeaks workout -rw --pr --users 22677 --exercise 326 --dates 2017-01-01 2017-12-31
```

```json
{
  "exercise": {
    "id": 326,
    "title": "Back Squat"
  },
  "users": [
    {
      "id": 22677,
      "name_first": "Barry",
      "name_last": "Moore",
      "personal_record": 5250,
      "record_stat": "total_weight",
      "record_date": "December 03 2017"
    }
  ]
}
```

#
> What is Abby Smith's all-time Bench Press PR (personal record) weight?

**Answer**: 350 lbs

```dos
.\trainingpeaks workout -w --pr --users 5101 --exercise 568
```

```json
{
  "exercise": {
    "id": 568,
    "title": "Bench Press"
  },
  "users": [
    {
      "id": 5101,
      "name_first": "Abby",
      "name_last": "Smith",
      "personal_record": 350,
      "record_stat": "weight",
      "record_date": "September 25 2016"
    }
  ]
}
```

## How To

#### Using `-r` and `-w` flags to find stats for a single user

The command below uses the `-r` flag to specify the search will be for exercise **reps**.  It's written in simplest form supported for this application. It can be interpreted as "Calculate the cumulative **reps** for user `5101` (Abby Smith) for exercise `568` (Bench Press)."  It will  include all workout history since no date range was provided.  
```dos
trainingpeaks workouts -r --users 5101 --exercise 568
```

To change the search critera to exercise **weight**, use the flag `-w`.
```dos
trainingpeaks workouts -w --users 5101 --exercise 568
```

To include multiple users, provide a comma-delimited list of users as an arument to the `--users` option.
```dos
trainingpeaks workouts -r --users 5101,9705 --exercise 568
```

Combining the `-rw` flags will find the cumulative **total weight** (**reps*****weight**) for user ID `5101` for exercise ID `568` for all of 2017.

```dos
.\trainingpeaks workout -rw --users 5101 --exercise 568 --dates 2017-01-01 2017-12-31
```
#

#### Using the `--pr` option to find the personal record for an exercise.
If you include the `--pr` option with the `-w`,  `-r`, or combined `-rw` flag, the app will find the **personal record** of the stats for the workout matching ID `568` (**Bench Press**) for the specific date provided.
```dos
.\trainingpeaks workout -w --pr --users 5101 --exercise 568 --dates 2017-01-10
```

#

#### Using `-o` option to save output to a JSON file
If you include the `-o` option, you must also include a file path and file name to save the output to a json file.  
```dos
.\trainingpeaks workout -rw --users 5101 --exercise 568 -o C:\MyData\output.json
```

#
#### Notes:
The `--dates` option can be omitted in order to consider a user's entire workout history.

Adding the `-W` flag (case sensitive) will print any warnings generated during the execution of a query.


## Finding IDs By Name  

```dos
.\trainingpeaks get-id -u Abby 
```
This command will look for a single user with partial name "Abby" and print their ID if a record of this user exists. 

```dos
.\trainingpeaks get-id -e Pulldown
```
This command will look for single exercise with partial name "Pulldown" and print the ID if it exists.

## Commands At A Glance

The console app works with the following two top-level commands.

### get-id
```dos
-u, --user        Specify user by full or partial name.
                  Valid formats:
                      - Jon Doe
                      - John
                      - Doe

-e, --exercise    Specify exercises by full or partial name.
                   Valid formats:
                      - Lat Pulldown
                      - Lat
                      - Pulldown
```

### workout

```dos
-r            (Group: Stats Flags) The amount reps completed for an exercise.

-w            (Group: Stats Flags) The amount of weight lifted for an exercise.

--pr          The personal record for an exercise.

--users       Required. Specify one or more users by ID.
              Multiple IDs must be comma delimited (e.g. 1234,9876,4545)

--exercise    Required. Specify an exercises by ID

--dates       Specify a date, or date range with format: yyyy-mm-dd.
               Valid ranges:
                2025-01-01            Finds workouts on a specific date.
                2025-01-01 2025-01-31 Finds workouts within a date range.

-W            Print Warnings

-o            Output file path.
```
