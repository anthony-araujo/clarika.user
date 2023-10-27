import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute } from "@angular/router";
import { of } from "rxjs";

import { LocationTypeDetailComponent } from "./location-type-detail.component";

describe("LocationType Management Detail Component", () => {
  let comp: LocationTypeDetailComponent;
  let fixture: ComponentFixture<LocationTypeDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LocationTypeDetailComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: { data: of({ locationType: { id: 123 } }) },
        },
      ],
    })
      .overrideTemplate(LocationTypeDetailComponent, "")
      .compileComponents();
    fixture = TestBed.createComponent(LocationTypeDetailComponent);
    comp = fixture.componentInstance;
  });

  describe("OnInit", () => {
    it("Should load locationType on init", () => {
      // WHEN
      comp.ngOnInit();

      // THEN
      expect(comp.locationType).toEqual(expect.objectContaining({ id: 123 }));
    });
  });
});
