import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute } from "@angular/router";
import { of } from "rxjs";

import { UserAppDetailComponent } from "./user-app-detail.component";

describe("UserApp Management Detail Component", () => {
  let comp: UserAppDetailComponent;
  let fixture: ComponentFixture<UserAppDetailComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserAppDetailComponent],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: { data: of({ userApp: { id: 123 } }) },
        },
      ],
    })
      .overrideTemplate(UserAppDetailComponent, "")
      .compileComponents();
    fixture = TestBed.createComponent(UserAppDetailComponent);
    comp = fixture.componentInstance;
  });

  describe("OnInit", () => {
    it("Should load userApp on init", () => {
      // WHEN
      comp.ngOnInit();

      // THEN
      expect(comp.userApp).toEqual(expect.objectContaining({ id: 123 }));
    });
  });
});
