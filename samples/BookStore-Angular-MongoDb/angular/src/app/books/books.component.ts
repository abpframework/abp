import { Component, OnInit } from "@angular/core";
import { Store } from "@ngxs/store";

@Component({
  selector: "app-books",
  templateUrl: "./books.component.html",
  styleUrls: ["./books.component.scss"]
})
export class BooksComponent implements OnInit {
  constructor(private store: Store) {}

  ngOnInit() {}
}
