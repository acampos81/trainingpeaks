# TrainingPeaks Console App

This console application can be used to lookup the following data for a single user, or list of users:

**Total Weight** - The total amount of weight a user has lifted for a given exercise, calculated by reps * weight.

**Personal Record** - The most amount of weight a user has lifted for a given exercise.

## QuickStart

#### Running Test Queries

```dos
trainingpeaks workouts --pr --users 5101 --exercise 568 --dates 2017-01-01 2017-12-31 -o C:/MyData/output.json
```
Running this command will fetch the personal record data for user `5101` for exercise ID `568` for all of 2017, and save the output to the file path `C:/MyData/output.json`.

```dos
trainingpeaks workouts --tw --users 5101 --exercise 568 --dates 2017-01-10
```
Running this command will fetch the total weight for a workout found on the specific date provided.  Ommitting the `-o` flag will skip writing the results to a file.


#### Notes:
The `--dates` option can be ommitted in order to consider a user's entire workout history.

Adding the `--w` option will print any warnings generated during the execution of a query.


### Finding IDs By Name  


```dos
trainingpeaks get-id -u Abby 
```
This command will look for a single user with partial name "Abby" and print their ID if a record of this user exists. 

```dos
trainingpeaks get-iu -e Pulldown
```
This command will look for single exercise with partial name "Pulldown" and print the ID if it exists.

## Total Weight JSON Output

The following command will look for **Total Weight** of `Abby Smith` and `Jamie Johnston` for all `Bench Press` exercises completed for the month of June 2018.

```dos
workout --tw --u 5101,29891 --e 568 --d 2018-06-01 2018-06-30
```

The JSON output will be:

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
      "total_weight": 128935
    },
    {
      "id": 29891,
      "name_first": "Jamie",
      "name_last": "Johnston",
      "total_weight": 159750
    }
  ],
  "combined_total_weight": 288685
}
```

## Personal Record JSON Output

The following command will look for **Personal Record** of `Jamie Johnston` for all `Bench Press` exercises completed on `03/05/2018`

```dos
workout --pr --u 29891 --e 568 --d 2018-03-05
```

The JSON output will be:

```json
{
  "exercise": {
    "id": 568,
    "title": "Bench Press"
  },
  "users": [
    {
      "id": 29891,
      "name_first": "Jamie",
      "name_last": "Johnston",
      "personal_record": 160
    }
  ]
}
```


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
-u, --users       Required. Specify one or more users by ID.
                    Multiple IDs must be comma delimeted (e.g. 1234,9876,4545)

-e, --exercise    Required. Specify an exercises by ID

-d, --dates       Specify a date, or date range with format: yyyy-mm-dd.
                   Valid ranges:
                    2025-01-01            Finds workouts on a specific date.
                    2025-01-01 2025-01-31 Finds workouts within a date range.


--tw              The total weight (reps * weight) for an exercise.

--pr              The most weight lifted for an exercise, regarldess of reps.

-w                Print Warnings

-o, --output      Output file path.
```