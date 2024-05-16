// Barber profile page for customer to use to see what time slots are available 
// Customer should pick a barber and service first
// Move this logic to the API AppointmentsController


Logic.GetAllAvailableAppointmentsByBarberId(45);


Console.WriteLine("Done!");

Console.ReadLine();

public static class Logic
{
    public static List<Appointment> GetAllAvailableAppointmentsByBarberId(int serviceDuraion)
    {


        //List<Appointment> bookedAppointments = new List<Appointment>()
        //{
        //    new Appointment()
        //    {
        //        AppointmentStartTime = new TimeOnly(10, 00),
        //        AppointmentEndTime = new TimeOnly(10, 30)
        //    },
        //    new Appointment()
        //    {
        //        AppointmentStartTime = new TimeOnly(10, 30),
        //        AppointmentEndTime = new TimeOnly(11, 00)
        //    },
        //    new Appointment()
        //    {
        //        AppointmentStartTime = new TimeOnly(11, 45),
        //        AppointmentEndTime = new TimeOnly(12, 15)
        //    },
        //    new Appointment()
        //    {
        //        AppointmentStartTime = new TimeOnly(12, 45),
        //        AppointmentEndTime = new TimeOnly(13, 15)
        //    }

        //};

        /*
         Result should be:

        Booked Appointments:
        10:00 AM - 10:30 AM
        10:30 AM - 11:00 AM
        -------------------> 11:00 AM - 11:45 AM
        11:45 AM - 12:15 PM
        12:45 PM - 1:15 PM
        -------------------> 1:15 PM - 2:00 PM
        2:00 PM - 3:00 PM
        ======================================
        Available Appointments:
        11:00 AM - 11:45 AM  <------
        1:15 PM - 2:00 PM    <------

        3:00 PM - 3:45 PM
        3:45 PM - 4:30 PM
        4:30 PM - 5:15 PM
        5:15 PM - 6:00 PM
        6:00 PM - 6:45 PM
        Done!
         
         */

        serviceDuraion = 30;
        List<Appointment> bookedAppointments = new List<Appointment>()
        {
            new Appointment()
            {
                AppointmentStartTime = new TimeOnly(10, 00),
                AppointmentEndTime = new TimeOnly(10, 30)
            },
            new Appointment()
            {
                AppointmentStartTime = new TimeOnly(10, 30),
                AppointmentEndTime = new TimeOnly(11, 00)
            },
            new Appointment()
            {
                AppointmentStartTime = new TimeOnly(11, 30),
                AppointmentEndTime = new TimeOnly(12, 00)
            },
            new Appointment()
            {
                AppointmentStartTime = new TimeOnly(12, 30),
                AppointmentEndTime = new TimeOnly(13, 00)
            }

        };

        /*
            Booked Appointments:
            10:00 AM - 10:30 AM
            10:30 AM - 11:00 AM
            -------------------->  11:00 AM - 11:30 AM
            11:30 AM - 12:00 PM
            --------------------> 12:00 PM - 12:30 PM
            12:30 PM - 1:00 PM
            -------------------> 1:00 PM - 1:30 PM
            -------------------> 1:30 PM - 2:00 PM  
            2:00 PM - 3:00 PM
            ======================================
            Available Appointments:
            11:00 AM - 11:30 AM   <----------
            12:00 PM - 12:30 PM
            1:00 PM - 1:30 PM    <-----------
            1:30 PM - 2:00 PM    <--------------- 
            3:00 PM - 3:30 PM
            3:30 PM - 4:00 PM
            4:00 PM - 4:30 PM
            4:30 PM - 5:00 PM
            5:00 PM - 5:30 PM
            5:30 PM - 6:00 PM
            6:00 PM - 6:30 PM
            6:30 PM - 7:00 PM
            Done!
         */



        var barberStartLunchTime = new TimeOnly(14, 00);
        var barberEndLunchTime = barberStartLunchTime.AddHours(1);




        // Get this info in separte call to db ti get info about the barber
        var barberStartTime = new TimeOnly(10, 00);
        var barberFinishTime = new TimeOnly(19, 00);

        int numberOfAppointments = bookedAppointments.Count;
        bookedAppointments.Add(new Appointment
        {
            AppointmentStartTime = barberStartLunchTime,
            AppointmentEndTime = barberEndLunchTime
        });


        Console.WriteLine("Booked Appointments: ");
        foreach (var appointment in bookedAppointments)
        {
            Console.WriteLine($"{appointment.AppointmentStartTime} - {appointment.AppointmentEndTime}");
        }


        DateTime startTime = Convert.ToDateTime(barberStartTime.ToString());
        DateTime finishTime = Convert.ToDateTime(barberFinishTime.ToString());

        TimeSpan startToFinishTimeSpan = finishTime - startTime;
        int timeInterval = (int)startToFinishTimeSpan.TotalMinutes;

        //Console.WriteLine($"Time span between barber starting and end day: {timeInterval}");

        List<Appointment> availableAppointments = new List<Appointment>();

        int numberOfTimeSlots = timeInterval / serviceDuraion;

        int i = 0;
        int j = 0;
        var startTimeCanChange = barberStartTime;
        var fromTime = new TimeOnly();
        var toTime = new TimeOnly();
        bool checkIfRunsbefore = false;

        do
        {
            var app = new Appointment();

            fromTime = startTimeCanChange.AddMinutes(serviceDuraion * i);
            // Console.WriteLine($"fromTime value {fromTime}");
            if (j < bookedAppointments.Count)
            {
                if (j > 0)
                {
                    TimeSpan diffBetweenTwoAppointments = bookedAppointments[j].AppointmentStartTime - bookedAppointments[j - 1].AppointmentEndTime;
                    int totalMinBetweenTwo = (int)diffBetweenTwoAppointments.TotalMinutes;
                    if (totalMinBetweenTwo >= serviceDuraion && fromTime != bookedAppointments[j - 1].AppointmentEndTime)
                    {
                        startTimeCanChange = bookedAppointments[j - 1].AppointmentEndTime;
                        fromTime = bookedAppointments[j - 1].AppointmentEndTime;
                        i--;
                        if (j == bookedAppointments.Count - 1)
                        {
                            checkIfRunsbefore = true;
                        }
                        //numberOfTimeSlots -= 1;
                    }
                    if (j == bookedAppointments.Count - 1)
                    {
                        if (checkIfRunsbefore)
                        {
                            j--;
                        }
                        else
                        {
                            startTimeCanChange = bookedAppointments[j].AppointmentEndTime;
                            i = -1; // numberOfTimeSlots -= 2;
                                    //numberOfTimeSlots -= (j + 2);
                        }

                    }



                }

                j++;
            }

            i++;
            toTime = fromTime.AddMinutes(serviceDuraion);
            if (toTime > barberFinishTime)
            {
                break;
            }

            var checkIfNotOverlap = CheckForOverlap(bookedAppointments, fromTime, toTime, serviceDuraion);

            if (checkIfNotOverlap)
            {
                app.AppointmentStartTime = fromTime;
                app.AppointmentEndTime = toTime;
                availableAppointments.Add(app);
                //Console.WriteLine($"{fromTime} - {toTime}  added to the available list");
            }


        } while (i < numberOfTimeSlots);


        Console.WriteLine("======================================");
        Console.WriteLine("Available Appointments: ");
        foreach (var appointment in availableAppointments)
        {
            Console.WriteLine($"{appointment.AppointmentStartTime} - {appointment.AppointmentEndTime}");
        }




        return availableAppointments;
    }

    private static bool CheckForOverlap(List<Appointment> bookedAppointments,
                TimeOnly fromTime, TimeOnly toTime, int serviceDuraion)
    {
        bool output = true;
        foreach (var time in bookedAppointments)
        {


            if ((fromTime < time.AppointmentStartTime && toTime <= time.AppointmentStartTime) ||
                fromTime >= time.AppointmentEndTime)
            {
                continue;
            }
            else
            {
                output = false;
            }



        }

        return output;
    }
}
public class Appointment
{
    public TimeOnly AppointmentStartTime { get; set; }
    public TimeOnly AppointmentEndTime { get; set; }



}