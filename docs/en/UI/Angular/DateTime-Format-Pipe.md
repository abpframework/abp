{%{
# DateTime Format Pipes

You can format date by Date pipe of angular.

Example

<span>{{today | date 'dd/mm/yy'}}</span>


ShortDate, ShortTime and ShortDateTime format data like angular's data pipe but easier. Also the pipes get format from config service by culture.

# ShortDate Pipe

<span>{{today | shortDatePipe }}</span>


# ShortTime Pipe

<span>{{today | shortTimePipe }}</span>


# ShortDateTime Pipe

<span>{{today | shortDateTimePipe }}</span>


}%} 
