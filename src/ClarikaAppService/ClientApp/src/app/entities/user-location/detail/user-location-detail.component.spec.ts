import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute } from "@angular/router";
import { of } from "rxjs";

import { UserLocationDetailComponent } from "./user-location-detail.component";

describe("UserLocation Management Detail Component", () => {
  let comp: UserLocationDetailComponent;
  let fixture: ComponentFixture<UserLocationDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserLocationDetailComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: { data: of({ userLocation: { id: 123 } }) },
        },
      ],
    })
      .overrideTemplate(UserLocationDetailComponent, "")
      .compileComponents();
    fixture = TestBed.createComponent(UserLocationDetailComponent);
    comp = fixture.componentInstance;
  });

  describe("OnInit", () => {
    it("Should load userLocation on init", () => {
      // WHEN
      comp.ngOnInit();

      // THEN
      expect(comp.userLocation).toEqual(expect.objectContaining({ id: 123 }));
    });
  });
});
